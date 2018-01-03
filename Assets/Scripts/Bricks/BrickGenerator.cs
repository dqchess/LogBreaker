using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGenerator : Brick {

    [Header("References")]

    [SerializeField]
    GameObject m_brickPrefab;

    [SerializeField]
    GameObject m_explosionEffect;

    Transform m_parentTransform;

    OrderCounter m_orderCounter;

    [Header("Variables")]

    [SerializeField]
    float m_offset = 0.1f;

    bool m_wasGenerated;

    Vector3[] m_bricksOrigins;

    int m_bricksAdded;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
        m_parentTransform = GameObject.FindGameObjectWithTag("Level Parent").transform;
    }

    void Initialize()
    {
        m_bricksOrigins = new Vector3[4];
        m_bricksAdded = 0;
        m_orderCounter = FindObjectOfType<OrderCounter>();
        m_bricksOrigins[0] = new Vector3(2f, 0f, 0f);
        m_bricksOrigins[1] = new Vector3(-2f, 0, 0f);
        m_bricksOrigins[2] = new Vector3(0f, .5f, 0f);
        m_bricksOrigins[3] = new Vector3(0f, -.5f, 0f);
    }

    protected override void DestroyBrick(GameObject _collidingBall = null)
    {
        if (!m_wasGenerated)
            StartCoroutine(GenerateBricksCO());     
    }

    IEnumerator GenerateBricksCO()
    {     
        m_wasGenerated = true;
        for (int i = 0; i < m_bricksOrigins.Length; i++)
        {
            yield return new WaitForSeconds(.5f);

            float distance = (i >= 2) ? .5f : 1.5f;


            if (!Physics2D.Raycast(transform.position, m_bricksOrigins[i], distance))
            {
                m_bricksAdded++;
                GameObject brick =  Instantiate(m_brickPrefab, transform.position + m_bricksOrigins[i], Quaternion.identity);
                brick.transform.SetParent(m_parentTransform, true);

                if(m_gameManager.m_LevelGoal == LEVEL_GOAL.ORDER)
                {
                    if (m_gameManager.m_DestroyAll)
                    {
                        
                       // m_orderCounter.UpdateTotal("All");
                    }
                    else
                    {
                        LevelOrder levelOrder = m_gameManager.GetLevelOrder(brick.tag);
                        if (levelOrder != null)
                        {
                            if (levelOrder.goal == Goal.All)
                            {
                                m_orderCounter.UpdateTotal(brick.tag);
                                levelOrder.count++;

                            }
                        }
                    }
                }
                              
            }                    
       }
        m_gameManager.IncreaseAllNumber(m_bricksAdded);

        if (HitPoints <= 0)
        {
            Instantiate(m_explosionEffect, transform.position, transform.rotation);
            base.DestroyBrick();
        }
    }  
}
