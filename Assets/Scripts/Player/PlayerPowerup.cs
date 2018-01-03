using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    MAKE_LONG,
    MAKE_SHORT,
    CHANGE_DIRECTION
}

public class PlayerPowerup : MonoBehaviour {
   
    Vector3 originalScale;

    GameManager m_gameManager;

    PlayerMovement m_playerMovment;

    [Header("Variables")]

    [SerializeField]
    public float ElongateSpeed;

    float m_longTime;

    float m_shortTime;

    void Awake()
    {
        GetReferences();
        originalScale = transform.localScale;
    }

    void GetReferences()
    {
        m_playerMovment = GetComponent<PlayerMovement>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (m_gameManager.m_GameState != GAMESTATE.PLAY)
            return;

        if(m_longTime > 0)
        {
            m_longTime -= Time.deltaTime;
            if (m_longTime <= 0)
            {              
                StartCoroutine( MakeShort(GameControl.control.m_LevelInfo.paddleScale));
            }
        }

        if (m_shortTime > 0)
        {
            m_shortTime -= Time.deltaTime;
            if (m_shortTime <= 0)
            {
               
                StartCoroutine(MakeLong(GameControl.control.m_LevelInfo.paddleScale));
            }
        }
    }

    public void ChangeScale(float time, float scale)
    {
       ChangeXScaleCO(time, scale);
    }

    void ChangeXScaleCO(float time, float scale)
    {
        bool elongate;
        elongate = scale > 1 ? true : false;
        scale *= GameControl.control.m_LevelInfo.paddleScale;

        if (elongate)
        {
           if(m_longTime <= 0)
            {
                StartCoroutine(MakeLong(scale));
            }
            m_longTime += time;                      
        }
        else
        {
           if(m_shortTime <= 0)
            {
                StartCoroutine(MakeShort(scale));
            }
            m_shortTime += time;                                
        }
    }

    IEnumerator MakeLong(float _scale)
    {        
        while (transform.localScale.x < _scale)
        {
            transform.localScale += Vector3.one * ElongateSpeed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MakeShort(float _scale)
    {
        while (transform.localScale.x > _scale)
        {
            transform.localScale -= Vector3.one * ElongateSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void ChangeDirection(float _time)
    {
        if (!m_playerMovment.m_IsReversedDirection)
        {
            m_playerMovment.m_IsReversedDirection = true;
        }
        else
        {
            m_playerMovment.m_IsReversedDirection = false;
        }                
    }

}
