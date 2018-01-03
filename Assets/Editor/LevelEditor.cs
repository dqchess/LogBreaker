using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;


[InitializeOnLoad]
public class LevelEditor : Editor {

    static List<Vector3> spawnPositions = new List<Vector3>();

    public int num = 2;

    static Grid m_grid;
    static Grid grid
    {
        get
        {
            if (m_grid == null)
            {
                GameObject go = GameObject.Find("Grid");

                if (go != null)
                {
                    m_grid = go.GetComponent<Grid>();
                }
            }

            return m_grid;
        }
    }

    static Transform m_LevelParent;
    static Transform LevelParent
    {
        get
        {
            if (m_LevelParent == null)
            {
                GameObject go = GameObject.Find("Level");

                if (go != null)
                {
                    m_LevelParent = go.transform;
                }
            }

            return m_LevelParent;
        }
    }

    public static int SelectedBlock
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorBlock", 0);
        }
        set
        {
            EditorPrefs.SetInt("SelectedEditorBlock", value);
        }
    }

    public static int SelectedTool
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorTool", 0);
        }
        set
        {
            if (value == SelectedTool)
            {
                return;
            }

            EditorPrefs.SetInt("SelectedEditorTool", value);

            switch (value)
            {
                case 0:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
            }
        }
    }

    static LevelBlocks m_LevelBlocks;
    static LevelLayout levelLayoutSO;

    static LevelEditor()
    {       
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        m_LevelBlocks = AssetDatabase.LoadAssetAtPath<LevelBlocks>("Assets/ScriptableObjects/LevelBlocks.asset");
        levelLayoutSO = AssetDatabase.LoadAssetAtPath<LevelLayout>("Assets/Resources/Static/new.asset");         
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (m_LevelBlocks == null)
        {
            //Debug.Log("No levelBlocks");
            return;
        }
        DrawCustomBlockButtons(sceneView);
        DrawToolsMenu(sceneView.position);
        HandleLevelEditorPlacement();
    }

    static void HandleLevelEditorPlacement()
    {
        if(SelectedTool == 0)
        {
            return;
        }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);      

        if (Event.current.type == EventType.mouseDown &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.shift == false &&
            Event.current.control == false)
        {
            Vector3 mousePos = Camera.current.ScreenPointToRay(
                new Vector3(Event.current.mousePosition.x, 
                -Event.current.mousePosition.y + Camera.current.pixelHeight)).origin;

            Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.Width) * grid.Width + grid.Width / 2.0f,
                                   Mathf.Floor(mousePos.y / grid.Height) * grid.Height + grid.Height / 2.0f, 0.0f);



            if (SelectedTool == 1)
            {
                RemoveBlock(aligned);
            }
            if (SelectedTool == 2 && m_LevelBlocks.Blocks[SelectedBlock].Name != "Border")
            {
                AddBlock(aligned, m_LevelBlocks.Blocks[SelectedBlock].Prefab);
            }
            if (SelectedTool == 3)
            {
                LevelLayout newLevelLayout = ScriptableObject.CreateInstance<LevelLayout>();
                for (int i = 0; i < levelLayoutSO.levelLayout.Count; i++)
                {
                    LevelElement element = new LevelElement();
                    element.Prefab = levelLayoutSO.levelLayout[i].Prefab;
                    element.Position = levelLayoutSO.levelLayout[i].Position;
                    newLevelLayout.levelLayout.Add(element);
                }
                // newLevelLayout = levelLayoutSO;
                int index = FindObjectOfType<LevelGenerator>().index;
                AssetDatabase.CreateAsset(newLevelLayout, "Assets/Resources/Static/" + index.ToString() +".asset");
            }
            if (SelectedTool == 4)
            {
                LevelGenerator generator = FindObjectOfType<LevelGenerator>();
                generator.GenerateLevelSO(generator.index);
                levelLayoutSO = generator.m_LevelLayoutSO;
            }
            if (SelectedTool == 5)
            {
                
                levelLayoutSO = AssetDatabase.LoadAssetAtPath<LevelLayout>("Assets/Resources/Static/new.asset");
            }

        }
        if (Event.current.type == EventType.keyDown &&
           Event.current.keyCode == KeyCode.Escape)
        {
            SelectedTool = 0;
        }

        HandleUtility.AddDefaultControl(controlId);
    }

    public static void RemoveBlock(Vector3 position)
    {
        for (int i = 0; i < LevelParent.childCount; ++i)
        {
            float distanceToBlock = Vector3.Distance(LevelParent.GetChild(i).transform.position, position);
            if (distanceToBlock < 0.1f)
            {
                RemoveFromLayout(position);
                spawnPositions.Remove(LevelParent.GetChild(i).transform.position);
                Undo.DestroyObjectImmediate(LevelParent.GetChild(i).gameObject);
                
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                return;
            }
        }
    }

    static void RemoveFromLayout(Vector3 pos)
    {
        for (int i = 0; i < levelLayoutSO.levelLayout.Count; i++)
        {
            if(levelLayoutSO.levelLayout[i].Position == pos)
            {
                levelLayoutSO.levelLayout.Remove(levelLayoutSO.levelLayout[i]);
            }
        }
    }

    public static void AddBlock(Vector3 position, GameObject prefab)
    {
        //if (spawnPositions.Contains(position))
        //{
        //    Debug.Log("cant add second one");
        //    return;
        //}
        if (prefab == null)
        {
            return;
        }

        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newCube.transform.parent = LevelParent;
        newCube.transform.position = position;

        Undo.RegisterCreatedObjectUndo(newCube, "Add " + prefab.name);

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        spawnPositions.Add(position);
        LevelElement element = new LevelElement();
        element.Prefab = prefab;
        element.Position = position;
        levelLayoutSO.levelLayout.Add(element);


    }

    static void DrawToolsMenu(Rect position)
    {      
        Handles.BeginGUI();
       
        GUILayout.BeginArea(new Rect(0, position.height - 35, position.width, 20), EditorStyles.toolbar);
        {
            string[] buttonLabels = new string[] { "None", "Erase", "Paint", "Create", "Load", "New" };

            SelectedTool = GUILayout.SelectionGrid(
                SelectedTool,
                buttonLabels,
                6,
                EditorStyles.toolbarButton,
                GUILayout.Width(600));
        }
        GUILayout.EndArea();

        Handles.EndGUI();
    }

    static void DrawCustomBlockButtons(SceneView sceneView)
    {
        Handles.BeginGUI();

        GUI.Box(new Rect(0, 0, 140, sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);

        
        for (int i = 0; i < m_LevelBlocks.Blocks.Count; i++)
        {
            DrawCustomBlockButton(i, sceneView.position);
        }
       
        Handles.EndGUI();
    }

    static void DrawCustomBlockButton(int index, Rect sceneViewRect)
    {
        bool isActive = false;

        if (SelectedTool == 2 && index == SelectedBlock)
        {
            isActive = true;
        }

        Texture2D previewImage = AssetPreview.GetAssetPreview(m_LevelBlocks.Blocks[index].Prefab);
        GUIContent buttonContent = new GUIContent(previewImage);

        Color defaultColor = GUI.color;
        float xOffset = 0;    
        int tempIndex = index;

        if(index >= m_LevelBlocks.Blocks.Count / 2)
        {
            xOffset = 60;
            tempIndex = index - 7;         
        }
       

        bool isToggleDown = GUI.Toggle(new Rect(xOffset + 5, 50 + tempIndex * 72 + 5, 50, 50), isActive, buttonContent, GUI.skin.button);
        GUI.color = Color.black;
        GUI.Label(new Rect(xOffset + 5, 50 + tempIndex * 72 + 55, 50, 20), m_LevelBlocks.Blocks[index].Name);
        GUI.color = defaultColor;

        if (isToggleDown == true && isActive == false)
        {
            SelectedBlock = index;
            SelectedTool = 2;
            if(m_LevelBlocks.Blocks[SelectedBlock].Name == "Border")
            {
                EditorWindow borderGeneratorWindow = BorderGenetarorWindow.ShowWindow();
                
            }
        }
    }
}
