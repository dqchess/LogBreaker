using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class Level : MonoBehaviour
{
    public int rows;
    public int columns;
    public GameObject[,] m_bricks;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (m_bricks == null || m_bricks.Length != columns * rows )
        {
            m_bricks = new GameObject[rows, columns];
            //Debug.Log("Empty array");
           
        }
        else
        {
           // Debug.Log("Not Empty array");
            return;
        }
    }
}
