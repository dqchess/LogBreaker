using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Text m_scoreText;

    [SerializeField]
    Text m_bestScoreText;

    [SerializeField]
    GameObject MovesCounter;

    [SerializeField]
    GameObject TimeCounter;

    [SerializeField]
    Text m_movesCounterText;

    [SerializeField]
    Text m_timeCounterText;

    [SerializeField]
    AudioClip m_clip;

    Animator m_scoreTextAnim;

    AudioSource m_audio;

    StarsFiller m_starFiller;

    GameManager m_gameManager;

    [Header("Variables")]

    [HideInInspector]
    public int m_Moves;

    [HideInInspector]
    public float m_Time;

    int m_lastTime;

    int m_score;

    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            UpdateScore();
        }
    }

    void Awake()
    {
        GetReferences();
        Initialize();
    }

    void Initialize()
    {
        Score = 0;
        if ((GameControl.control.m_ActiveLevelScore == 1))
        {
            m_bestScoreText.text = "0";
        }
        else
        {
            m_bestScoreText.text = (GameControl.control.m_ActiveLevelScore).ToString();
        }
    }

    void GetReferences()
    {
        m_movesCounterText = MovesCounter.GetComponentInChildren<Text>();
        m_timeCounterText = TimeCounter.GetComponentInChildren<Text>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_starFiller = FindObjectOfType<StarsFiller>();
        m_audio = GetComponent<AudioSource>();
        m_scoreTextAnim = m_scoreText.GetComponent<Animator>();
    }

    public void PlayEffect()
    {
        m_starFiller.PlayEffectAtPoition();
    }

    void UpdateScore()
    {
        if(m_score > 0)
        {
            m_audio.PlayOneShot(m_clip, .2f);
            m_starFiller.FillStars(Score);
            m_scoreTextAnim.Play("ScoreChange", -1, 0f);
        }           
        m_scoreText.text = Score.ToString();     
    }

    public void SetRestriction(LevelRestriction _levelRestriction)
    {
        if(_levelRestriction == LevelRestriction.MOVES)
        {           
            m_Moves = GameControl.control.m_LevelInfo.moves;
            m_movesCounterText.text = m_Moves.ToString();
            MovesCounter.SetActive(true);
        }
        else if (_levelRestriction == LevelRestriction.TIME)
        {
            
            m_Time = GameControl.control.m_LevelInfo.time;
            m_timeCounterText.text = m_Time.ToString("0");
            
            TimeCounter.SetActive(true);
            m_lastTime = Mathf.FloorToInt(m_Time);
        }
    }

    void Update()
    {
        if(m_gameManager.m_GameState == GAMESTATE.PLAY && GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.TIME)
        {
            m_Time -= Time.deltaTime;
            UpdateTimeCounters();
            if (m_Time <= 0)
            {
                if(m_gameManager.m_LevelGoal == LEVEL_GOAL.POINTS && m_score >= GameControl.control.m_LevelInfo.PointsToAchieve)
                {
                    m_gameManager.Win();
                }
                else
                {
                    m_gameManager.GameOver();
                }                
            }
        }
    }

    public void TakeMove()
    {
        if (m_gameManager.m_GameState != GAMESTATE.PLAY)
            return;
        
        if (m_Moves <= 0)
        {
            if (m_gameManager.m_LevelGoal == LEVEL_GOAL.POINTS && m_score >= GameControl.control.m_LevelInfo.PointsToAchieve)
            {
                m_gameManager.Win();
            }
            else
            {
                m_gameManager.GameOver();
            }
        }
        else
        {
            m_Moves--;
            UpdateMovesCounters();
        }              
    }

    public void UpdateMovesCounters()
    {
        m_movesCounterText.text = m_Moves.ToString();       
    }

    public void UpdateTimeCounters()
    {
        if (Mathf.FloorToInt(m_Time) != m_lastTime)
        {
            m_timeCounterText.text = m_Time.ToString("0");
            m_lastTime = Mathf.FloorToInt(m_Time);
        }       
    }
}

