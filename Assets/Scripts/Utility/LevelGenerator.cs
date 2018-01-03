using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelGenerator : MonoBehaviour {

    [HideInInspector]
    public LevelLayout m_LevelLayoutSO;

    LevelMovers m_movingBricksSO;

    GameObject m_level;

    GameManager m_gameManager;

    [Header("Variables")]

    public int index;

    [SerializeField]
    bool m_genereteFromSO;

    void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        if (m_genereteFromSO)
        {
            GenerateLevelSO(GameControl.control.m_LevelIndex);
        }
        else
        {
            GenerateLevel();
        }            
    }

    public void GenerateLevelSO(int index)
    {
        m_LevelLayoutSO = Resources.Load<LevelLayout>("Static/" + index.ToString());
        List<LevelElement> levelElementsList = m_LevelLayoutSO.levelLayout;
        GameObject[] gos = new GameObject[levelElementsList.Count];

        for (int i = 0; i < levelElementsList.Count; i++)
        {
            Renderer prefabRenderer = levelElementsList[i].Prefab.GetComponentInChildren<Renderer>();
 
            GameObject go = Instantiate(levelElementsList[i].Prefab, levelElementsList[i].Position, levelElementsList[i].Prefab.transform.rotation);
            go.transform.SetParent(transform, true);

            if (prefabRenderer)
            {
                go.GetComponentInChildren<Renderer>().sharedMaterial = prefabRenderer.sharedMaterial;
            }
            gos[i] = go;

        }
        StaticBatchingUtility.Combine(gos, gameObject);
        GenerateMovingBricks(index);

    }


    void GenerateMovingBricks(int index)
    {
        m_movingBricksSO =  Resources.Load<LevelMovers>("Dynamic/" + index);

        if(m_movingBricksSO == null)
        {

            return;
        }

        List<MovingBrick> movingBricksList = m_movingBricksSO.movingBricksList;

        for (int i = 0; i < movingBricksList.Count; i++)
        {
            BezierSpline spline = Instantiate(movingBricksList[i].spline);
            GameObject go = Instantiate(movingBricksList[i].brick, spline.transform.position, movingBricksList[i].brick.transform.rotation);
            
            SplineWalker walker = go.GetComponent<SplineWalker>();
            if (walker)
            {
                walker.spline = spline;
                walker.mode = movingBricksList[i].mode;
                walker.duration = movingBricksList[i].duration;
            }
           
        }
    }

    void GenerateLevel()
    {      
        m_level = Resources.Load<GameObject>(GameControl.control.m_LevelIndex.ToString());
        Instantiate(m_level);
    }
}
