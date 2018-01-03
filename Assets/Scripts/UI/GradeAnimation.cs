using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GRADE
{
    GOOD,
    GREAT,
    FANTASTIC
}

public class GradeAnimation : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GameObject m_flowerFallPrefab;

    [SerializeField]
    GameObject m_ballTriggerPrefab;

    [SerializeField]
    PointsMesh m_pointsPrefab;

    Text m_text;

    RectTransform m_transform;

    GameManager m_gameManager;

    [Header("Variables")]

    [SerializeField]
    int m_goodPoints = 50;

    [SerializeField]
    public Vector3 m_poweupOrigin;

    [SerializeField]
    public Color m_goodColor;

    [SerializeField]
    public Color m_greatColor;

    [SerializeField]
    public Color m_fantasticColor;

    Color m_color;

    GRADE grade;

    Vector3 m_startingPos;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_transform = GetComponent<RectTransform>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_text = GetComponent<Text>();
    }

    void Start()
    {
        m_startingPos = transform.position;
    }

    public void PlayAnimation(int _hits)
    {
        if(_hits < 5)
        {
            grade = GRADE.GOOD;
            m_text.text = "Good";
            m_color = m_goodColor;
            m_text.color = m_goodColor;
           
        }
        else if (_hits < 8)
        {
            grade = GRADE.GREAT;
            m_text.text = "Great";
            m_color = m_greatColor;
            m_text.color = m_greatColor;
           
        }
        else
        {
            grade = GRADE.FANTASTIC;
            m_text.text = "Fantastic";
            m_color = m_fantasticColor;
            m_text.color = m_fantasticColor;
            
        }
        //m_anim.Play("Grade", -1, 0f);
        PlayTweenAnim();     
    }

    void PlayTweenAnim()
    {
        StartCoroutine(AnimCO());
    }

    IEnumerator AnimCO()
    {
        Vector3 VectorOne = Vector3.one;
        while (m_transform.localScale.x < 1f)
        {
            m_transform.localScale += Vector3.one * Time.deltaTime * 5f;
            yield return null;
        }

        Vector2 VectorUp = Vector2.up;
        Vector2 toPos = m_transform.anchoredPosition + VectorUp * 50f;
        while (m_transform.anchoredPosition.y < toPos.y)
        {
            m_transform.anchoredPosition += VectorUp * (50 * Time.deltaTime);

            yield return null;
        }
        GeneratePoints();

        while (m_transform.localScale.x > 0)
        {
            m_transform.localScale -= VectorOne * Time.deltaTime * 5f;

            yield return null;
        }
        m_transform.localScale = Vector3.zero;
        m_transform.position = m_startingPos;
    }

    public void GeneratePoints()
    {
        switch (grade)
        {
            case GRADE.GOOD:
                MakePoints(m_goodPoints, true);
                
                break;
            case GRADE.GREAT:
                if (m_gameManager.m_GameState == GAMESTATE.WIN || m_gameManager.m_GameState == GAMESTATE.BOOST)
                {
                    MakePoints(m_goodPoints, true);
                }
                else
                {
                    MakePoints(m_goodPoints, false);
                    MakeBall();
                }
               
                break;
            case GRADE.FANTASTIC:
                if (m_gameManager.m_GameState == GAMESTATE.WIN || m_gameManager.m_GameState == GAMESTATE.BOOST)
                {
                    MakePoints(m_goodPoints, true);
                }
                else
                {
                    MakePoints(m_goodPoints, false);
                    MakeShoot();
                }

                break;
            default:
                break;
        }      
    }

    void MakeShoot()
    {
         Instantiate(m_flowerFallPrefab, m_poweupOrigin, Quaternion.identity);       
    }

    void MakeBall()
    {
        Instantiate(m_ballTriggerPrefab, m_poweupOrigin, Quaternion.identity);            
    }

    void MakePoints(int _points, bool _addPoints)
    {
        PointsMesh pointsMesh = Instantiate(m_pointsPrefab, m_poweupOrigin, Quaternion.identity);
        pointsMesh.GetComponent<TextMesh>().color = new Color(m_color.r, m_color.g, m_color.b, 1f);
        
        pointsMesh.Initialize(_points, new Color(m_color.r, m_color.g, m_color.b, 1f), _addPoints);
    }
}
