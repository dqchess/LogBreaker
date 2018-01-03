using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsMesh : MonoBehaviour {

    ScoreManager m_scoreManager;

    int m_points;

    bool isTime;

    void Awake()
    {
        m_scoreManager = FindObjectOfType<ScoreManager>();     
    }

    public void Initialize(int points, Color color, bool addPoints)
    {
        TextMesh textMesh = GetComponent<TextMesh>();
        textMesh.color = color;
        m_points = points;
        if (!addPoints)
        {
            if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.TIME)
            {
                m_points = Mathf.RoundToInt((float)points / 10f);
                isTime = true;
                textMesh.text = "+" + m_points.ToString() + "s";
            }
            else
            {
                textMesh.text = "+" + m_points.ToString();
            }
        }    
        else
        {
            textMesh.text = "+" + m_points.ToString();
        }       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isTime)
            {
                m_scoreManager.m_Time += m_points;
            }
            else
            {
                m_scoreManager.Score += m_points;
            }
            
            gameObject.SetActive(false);            
        }

        else if(other.CompareTag("Bottom Border"))
        {
            gameObject.SetActive(false);
        }
    }
}
