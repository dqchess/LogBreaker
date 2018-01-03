using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using UnityEngine.UI;
using System;

public enum LevelRestriction
{
    NONE,
    MOVES,
    TIME,
}

public enum Goal
{
    All,
    SPECIFIC,
}

[Serializable]
public class LevelOrder
{   
    public string Tag;
    public int count;
    public Goal goal;
}

[System.Serializable]
public class LevelInfo
{
    public bool FreeBall;
    public GameObject FreeCOllider;
    public int PointsToAchieve;
    public List<LevelOrder> levelOrderList;   
    public LevelRestriction levelRestriction;  
    public float time;
    public int moves;
    public int lives;
    public float ballSpeed;   
    public float paddleScale;
    public float paddleSpeed;
}

[System.Serializable]
public class GoalDescription
{
    public string Description;
    public Sprite[] Sprites;

}

public class LevelButton : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GoalDescription goalDescription;

    [SerializeField]
    Sprite[] LevelSpritesArrey;

    [SerializeField]
    LevelInfo levelInfo;

    [SerializeField]
    GameObject LockedPopup;

    [SerializeField]
    GameObject StartLevelPopup;

    [SerializeField]
    GameObject NumberImage;

    [SerializeField]
    GameObject NewSticker;

    PopupOpener m_popupOpener;

    Image m_image;

    [Header("Variables")]

    [SerializeField]
    int WorldNum;

    [SerializeField]
    int LevelNum;

    [SerializeField]
    bool m_debugMode;

    int m_score;

    int m_starsCount;

    int m_state;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_image = GetComponentInChildren<Image>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        m_score = GameControl.control.GetScore(WorldNum, LevelNum);
        int index = (WorldNum - 1) * 10 + (LevelNum) - 1;
        m_starsCount = GameControl.control.GetStars(m_score, GameControl.control.m_LevelsStarValuesSO.LevelsStarValues[index]);

        if (LevelNum == 1 && WorldNum == 1 && m_score == 0)
        {
            m_state = 1;
        }

        else if (m_score == 0)
        {
            m_state = 0;
            NumberImage.SetActive(false);
        }

        else if (m_score == 1)
        {
            m_state = 1;
        }

        else
        {
            m_state = m_starsCount + 1;
        }

        m_image.sprite = LevelSpritesArrey[m_state];
        if (m_state == 1)
        {
            NewSticker.SetActive(true);
        }
    }

    public void ShowPopup()
    {
        GameControl.control.m_ActiveLevelScore = m_score;
        GameControl.control.m_ActiveLevelStars = m_starsCount;
        
        if (m_state == 0 && !m_debugMode)
        {
            m_popupOpener.popupPrefab = LockedPopup;
        }
        else
        {
            GameControl.control.m_GoalDescription = goalDescription;
            m_popupOpener.popupPrefab = StartLevelPopup;
            GameControl.control.m_LevelInfo = levelInfo;
            GameControl.control.m_ActiveWorldNum = WorldNum;
            GameControl.control.m_ActiveLevelNum = LevelNum;
            int index = (WorldNum - 1) * 10 + (LevelNum);
            GameControl.control.m_LevelIndex = index;
            GameControl.control.m_StarsValues = GameControl.control.m_LevelsStarValuesSO.LevelsStarValues[index - 1];
        }
        m_popupOpener.OpenPopup();
    }

    
}
