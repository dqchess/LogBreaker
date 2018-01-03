using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

    private GameManager m_gameManager;

    private EditorWindow m_levelEditor;

    public GameObject m_bricks;
    

    private void OnEnable()
    {
        m_gameManager = target as GameManager;
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Play"))
        {
            //m_gameManager.StartGameCO();
        }
        //if (GUILayout.Button("Level Editor"))
        //{
        //    m_levelEditor = LevelEditor.ShowWindow2();
            
        //}

        //if (GUILayout.Button("Close Level Editor"))
        //{
        //    m_levelEditor.Close();
        //}
    }
  
}
