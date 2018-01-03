using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;
using System;

public class WinScreen : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GameObject[] m_stars;

    [SerializeField]
    RectTransform[] m_sideStars;

    [SerializeField]
    Text m_scoreText;

    [SerializeField]
    GameObject m_adButton;

    [SerializeField]
    GameObject m_levelsButton;
     
    [SerializeField]
    Text m_debugText;

    [SerializeField]
    Text m_levelNumText;

    ScoreManager m_scoreManager;

    SceneTransition m_sceneTransition;

    GameManager m_gameManager;

    [Header("Variables")]

    int m_numOfStars;

    bool m_doubledPoints;


    void Awake()
    {
        GetReferences();
        m_levelNumText.text = GameControl.control.m_LevelIndex.ToString();
        m_scoreText.text = m_scoreManager.Score.ToString();
    }

    void GetReferences()
    {
        m_sceneTransition = GetComponent<SceneTransition>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_gameManager = FindObjectOfType<GameManager>();             
    }

    public void StartStars()
    {
        StartCoroutine(StartStarsCO());
    }

    IEnumerator StartStarsCO()
    {
        CheckIfScoreUnderOneStar();

        int livesLeft = m_gameManager.ActiveLifes - 1;

        if (livesLeft >= 1)
        {

            for (int i = livesLeft - 1; i >= 0; i--)
            {
                m_gameManager.DeactiveteLifeImage(i);
                m_scoreManager.Score += 50;
                m_scoreText.text = m_scoreManager.Score.ToString();
                yield return new WaitForSeconds(1f);
            }
        }

        if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.MOVES && !m_gameManager.m_MovesAdded)
        {
            if (m_scoreManager.m_Moves > 0)
            {
                SetDebugText(livesLeft, m_scoreManager.Score, m_scoreManager.m_Moves);

                int length = m_scoreManager.m_Moves;
                for (int i = 0; i < length; i++)
                {
                    m_scoreManager.Score += 10;
                    m_scoreManager.m_Moves--;
                    m_scoreText.text = m_scoreManager.Score.ToString();
                    yield return new WaitForSeconds(.1f);
                    m_scoreManager.UpdateMovesCounters();
                }

                m_scoreManager.m_Moves = 0;
                m_scoreManager.UpdateMovesCounters();
                yield return new WaitForSeconds(1f);
            }
        }

        else if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.TIME && !m_gameManager.m_MovesAdded)
        {
            if (m_scoreManager.m_Time > 0)
            {
                SetDebugText(livesLeft, m_scoreManager.Score, Mathf.RoundToInt(m_scoreManager.m_Time));

                int length = Mathf.RoundToInt(m_scoreManager.m_Time);
                for (int i = 0; i < length; i++)
                {
                    m_scoreManager.Score += 5;
                    m_scoreManager.m_Time--;
                    m_scoreText.text = m_scoreManager.Score.ToString();
                    m_scoreManager.UpdateTimeCounters();
                    yield return new WaitForSeconds(.1f);
                }
                m_scoreManager.m_Time = 0;
                m_scoreManager.UpdateTimeCounters();

                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            //SetDebugText(livesLeft, m_scoreManager.Score, 0);
        }

        m_numOfStars = GameControl.control.GetStars(m_scoreManager.Score, GameControl.control.m_StarsValues);

        for (int i = 0; i < m_numOfStars; i++)
        {
            m_stars[i].SetActive(true);
            yield return new WaitForSeconds(.5f);
        }
        for (int i = 0; i < 2; i++)
        {
            m_sideStars[i].gameObject.SetActive(true);
        }

        m_scoreManager.PlayEffect();
        SetAdButton();
        GameControl.control.AdCoins(m_scoreManager.Score);
        GameControl.control.SetBestScore(m_scoreManager.Score);
    }

    void CheckIfScoreUnderOneStar()
    {
        if (m_scoreManager.Score < GameControl.control.m_StarsValues.Star1)
        {
            m_scoreManager.Score = GameControl.control.m_StarsValues.Star1;
            m_scoreText.text = m_scoreManager.Score.ToString();
        }
    }

    void SetDebugText(int _livesLeft, int _score, int _timeMoves)
    {
        m_debugText.text = "Result: " + _score + "\nLives left :" + _livesLeft + "\nMoves/Time: " + _timeMoves; 
    }

    public void SetAdButton()
    {       
        m_levelsButton.SetActive(true);
        if (m_numOfStars == 2 || m_numOfStars == 3)
        {
            m_adButton.SetActive(true);
            CustomEventsManager.CanDoublePoints();
        }
        else
        {           
            m_levelsButton.transform.position = m_adButton.transform.position;
        }
    }

    public void PlayAd()
    {
        if (AdManager.Instance.m_IsPlayingAd || m_doubledPoints)
            return;
        m_doubledPoints = true;
        CustomEventsManager.DoubledPoints();
        AdManager.Instance.m_AdReward = AD_REWARD.DOUBLE_POINTS;
        AdManager.Instance.ShowAd();      
    }

    public void GoToLevelsScene()
    {
        m_sceneTransition.PerformTransition();
    }

    public void RewardDoublePoints()
    {
        GameControl.control.AdCoins(GameControl.control.m_CointToAdd);
        m_scoreManager.Score *= 2;
        m_scoreText.text = m_scoreManager.Score.ToString();
        m_adButton.gameObject.SetActive(false);
        m_levelsButton.SetActive(true);
        m_levelsButton.transform.position = m_adButton.transform.position;
    }
}
