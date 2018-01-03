using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrickSize
{
    Narrow,
    Wide,
    SuperWide,
    SuperNarrow
}

public class BorderGenerator : MonoBehaviour{

    [HideInInspector]
    public int wallWidth;
    [HideInInspector]
    public int wallHeight;
    [HideInInspector]
    public float brickHeight;

    private Transform m_borderParent;
    public Transform BorderParent
    {
        get
        {
            if (m_borderParent == null)
            {
                m_borderParent = new GameObject("Border").transform;
                m_borderParent.position = Vector3.zero;
            }
            return m_borderParent;

        }

    }


    public GameObject narrowBrick;
    public GameObject wideBrick;



    private List<GameObject> bricks = new List<GameObject>();


    private Dictionary<BrickSize, float> sizeValues = new Dictionary<BrickSize, float>
        {
            {BrickSize.Narrow, 1f},
            {BrickSize.Wide, 2f},
          
        };
    

    public void UpdateParameters(
            int wallWidth,
            int wallHeight
            )
    {
        this.wallWidth = wallWidth;
        this.wallHeight = wallHeight;

    }

    public void ApplyWall()
    {
        bricks.Clear();
        m_borderParent = null;
    }

    public void GenerateWall()
    {      
        foreach (var brick in bricks)
        {
            UnityEngine.Object.DestroyImmediate(brick);
        }
        bricks.Clear();

        for (int y = 0; y < wallHeight; y++)
        {
            List<BrickSize> brickSizes = FillWallWithBricks(wallWidth);

            Vector3 leftEdge = Vector3.left * wallWidth / 2 + Vector3.up * (y * brickHeight);

            for (int i = 0; i < brickSizes.Count; i++)
            {
                BrickSize brickSize = brickSizes[i];
                Vector3 position = leftEdge + Vector3.right * sizeValues[brickSize] / 2;

                GameObject brick = GenerateBrick(position, brickSize);
                bricks.Add(brick);
                brick.transform.parent = BorderParent;
                leftEdge.x += sizeValues[brickSize];
            }
        }
    }

    GameObject GenerateBrick(Vector3 position, BrickSize brickSize)
    {
        GameObject brick;

        switch (brickSize)
        {                                   
            case BrickSize.Narrow:
                brick = (GameObject)Instantiate(narrowBrick, position, Quaternion.identity);
                return brick;
            case BrickSize.Wide:
                brick = (GameObject)Instantiate(wideBrick, position, Quaternion.identity);
                return brick;      
            default:
                return null;
        }                
    }

    List<BrickSize> FillWallWithBricks(float width)
    {
        Dictionary<BrickSize, int> knapsack;
        float knapsackWidth;
        do
        {
            knapsack = GetRandomKnapsack(width);

            knapsackWidth = KnapsackWidth(knapsack);
        }
        while (knapsackWidth > width);

        width -= knapsackWidth;
        knapsack = Utility.Knapsack(sizeValues, width, knapsack);

        var brickSizes = new List<BrickSize>();
        foreach (var pair in knapsack)
        {
            for (var i = 0; i < pair.Value; i++)
            {
                brickSizes.Add(pair.Key);
            }
        }
        brickSizes.Shuffle();
        return brickSizes;
    }

    float KnapsackWidth(Dictionary<BrickSize, int> knapsack)
    {
        float knapsackWidth = 0f;
        foreach (var key in knapsack.Keys)
        {
            knapsackWidth += knapsack[key] * sizeValues[key];
        }
        return knapsackWidth;
    }

    Dictionary<BrickSize, int> GetRandomKnapsack(float width)
    {
        Dictionary<BrickSize, int> knapsack = new Dictionary<BrickSize, int>();
        foreach (var key in sizeValues.Keys)
        {
            knapsack[key] = (int)UnityEngine.Random.Range(0, width / 3);
        }
        return knapsack;
    }




}
