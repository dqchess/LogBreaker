using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System;
using ExaGames.Common;
using System.Linq;

public enum LEVEL_GOAL
{
    ORDER,
    POINTS,
    FREE_BALL
}

public enum GAMESTATE
{
    INIT,
    PLAY,
    WIN,
    LOSE,
    BOOST
}

public class GameManager : MonoBehaviour {

    string ALL = "All";
    string BRICK_LOG = "Brick Log";
    string BRICK_STONE = "Brick Stone";
    string BRICK_CAGE = "Brick Cage";
    string BRICK_BOMB = "Brick Bomb";
    string BRICK_SHOOT = "Brick Shoot";
    string BRICK_GENERATOR = "Brick Generator";
    string BRICK_CHANGE = "Brick Change";
    string BRICK_LONG = "Brick Long";
    string BRICK_SHORT = "Brick Short";

    [Header("References")]
    [Space(10)]

    [Header("Game Objects")]
    public GameObject m_LevelParenTGO;

    [SerializeField]
    GameObject m_livesImagesParentGO;

    [SerializeField]
    GameObject m_pauseButtonGO;

    [SerializeField]
    GameObject m_quitButtonGO;

    [SerializeField]
    LoseScreen m_loseScreen;

    [SerializeField]
    WinScreen m_winScreen;

    [Header("Prefabs")]

    [SerializeField]
    Ball m_ballPrefab;

    [SerializeField]
    GameObject m_bombBrickPrefab;

    [SerializeField]
    GameObject m_shootBrickPrefab;

    [SerializeField]
    GameObject m_ballBrickPrefab;

    [HideInInspector]
    public List<Ball> m_BallsList;

    AudioSource m_audio;

    GoalAnim m_goalAnim;

    GradeAnimation m_gradeAnimation;

    LivesManager m_livesManager;

    GameObject m_powerUpBrick;

    ScoreManager m_scoreManager;

    BrickStandard[] m_standardBricks;

    List<LevelOrder> m_levelOrder;

    PowerUpsManager m_powerupsManager;
  
    OrderCounter m_orderCounter;

    [Header("Variables")]

    [SerializeField]
    public Vector3 m_startingBallPos;

    [HideInInspector]
    public GAMESTATE m_GameState;

    [HideInInspector]
    public bool m_DestroyAll;

    [HideInInspector]
    public LEVEL_GOAL m_LevelGoal;

    [HideInInspector]
    public bool m_MovesAdded;

    int m_bricksToDestroy;

    bool m_isCheckingGrade; 

    int m_activeBalls; 

    int m_activeLives;

    public int ActiveLifes
    {
        get { return m_activeLives; }
        set
        {
            m_activeLives = value;
            if(m_activeLives <= 0)
            {

                GameOver();
            }
        }
    }

    public int ActiveBalls
    {
        get { return m_activeBalls; }
        set
        {
            m_activeBalls = value;
            if (m_activeBalls <= 0)
            {
                TakeNextLife();
            }
        }
    }

    void Awake()
    {
        GetReferences();
        FindObjectOfType<Ball>().RestetOnPlayerDeath();      
        m_goalAnim.gameObject.SetActive(false);
        m_levelOrder = GameControl.control.m_LevelInfo.levelOrderList;       
        m_BallsList = new List<Ball>();
        ActiveBalls = 1;
        CustomEventsManager.LevelStartedEvent(GameControl.control.m_LevelIndex);      
             
    }

    void GetReferences()
    {
        m_audio = GetComponent<AudioSource>();
        m_goalAnim = FindObjectOfType<GoalAnim>();
        m_orderCounter = FindObjectOfType<OrderCounter>();
        m_powerupsManager = FindObjectOfType<PowerUpsManager>();
        m_LevelParenTGO = GameObject.FindGameObjectWithTag("Level Parent");
        m_gradeAnimation = FindObjectOfType<GradeAnimation>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_livesManager = FindObjectOfType<LivesManager>();
    }

    public void IncreaseAllNumber(int _bricksAdded)
    {
        if (m_DestroyAll)
        {
            m_levelOrder[0].count += _bricksAdded;
        }
    }

    public void Initialize()
    {
        m_goalAnim.gameObject.SetActive(true);
        m_goalAnim.Initialize(GameControl.control.m_GoalDescription);
        m_standardBricks = FindObjectsOfType<BrickStandard>();
        m_scoreManager.SetRestriction(GameControl.control.m_LevelInfo.levelRestriction);
        HandlePowerUps();
        SetLevelWinContitions();
        SetLives();
    }

    public void LaunchBallAtStart()
    {
        Ball ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        ball.InitializeBall(m_startingBallPos, GetRandomStartDirection(), 1f, false);
        m_BallsList.Add(ball);
        ActivatePauseButtons();
    }

    void ActivatePauseButtons()
    {
        m_pauseButtonGO.SetActive(true);
        m_quitButtonGO.SetActive(true);
    }

    public void HandlePurchase()
    {
        if (ActiveLifes <= 0)
        {
            ActiveLifes++;
            m_livesImagesParentGO.transform.GetChild(ActiveLifes - 1).gameObject.SetActive(false);
            MakeBall(1f, new Vector3(1f, 1.25f, 0f), Vector2.up);            
        }

        else if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.MOVES)
        {
            m_MovesAdded = true;
            m_scoreManager.m_Moves += 15;
            StartCoroutine(ResumeCO(Vector3.up));
            m_scoreManager.UpdateMovesCounters();
        }

        else if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.TIME)
        {
            m_MovesAdded = true;
            m_scoreManager.m_Time += 30;
            StartCoroutine(ResumeCO(GetRandomStartDirection()));
            m_scoreManager.UpdateTimeCounters();
        }
        Ball.PlayerRestart();
        m_GameState = GAMESTATE.PLAY;
        
        m_loseScreen.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator ResumeCO(Vector2 _direction)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < m_BallsList.Count; i++)
        {
            m_BallsList[i].ResumeBall(_direction);
        }
    }

    public void CheckIfValidDestruction(string _tag)
    {
        switch (m_LevelGoal)
        {
            case LEVEL_GOAL.ORDER:
                if (m_DestroyAll)
                {
                    _tag = "All";
                }
                LevelOrder levelOrder = GetLevelOrder(_tag);
                
                if (levelOrder != null)
                {
                    levelOrder.count--;
                    if (levelOrder.count >= 0 && !m_DestroyAll)
                    {
                        m_orderCounter.UpdateCounter(_tag);
                    }                                  
                    levelOrder.count = Mathf.Clamp(levelOrder.count, 0, 1000);                                    
                    CheckWinConditions();
                }
                break;
            case LEVEL_GOAL.POINTS:
                m_bricksToDestroy--;
                CheckWinConditions();
                break;
        }
    }

    void SetLevelWinContitions()
    {
        if (GameControl.control.m_LevelInfo.FreeBall)
        {
            m_LevelGoal = LEVEL_GOAL.FREE_BALL;
            m_orderCounter.gameObject.SetActive(false);

        }
        else if (GameControl.control.m_LevelInfo.PointsToAchieve > 0)
        {
            m_orderCounter.gameObject.SetActive(false);
            m_bricksToDestroy = FindObjectsOfType<Brick>().Length;
            m_LevelGoal = LEVEL_GOAL.POINTS;
        }
        else
        {                     
            m_LevelGoal = LEVEL_GOAL.ORDER;

            m_DestroyAll = GameControl.control.m_LevelInfo.levelOrderList[0].Tag == ALL &&
            GameControl.control.m_LevelInfo.levelOrderList[0].goal == Goal.All;

            if (m_DestroyAll)
            {
                Brick[] bricksArray = FindObjectsOfType<Brick>();
                m_orderCounter.gameObject.SetActive(false);
                m_levelOrder[0].count = bricksArray.Length;
            }
            else
            {
                for (int i = 0; i < m_levelOrder.Count; i++)
                {

                    if (m_levelOrder[i].goal == Goal.All)
                    {
                        m_levelOrder[i].count = GameObject.FindGameObjectsWithTag(m_levelOrder[i].Tag).Length;
                    }

                }
                m_orderCounter.SetOrder(m_levelOrder);
            }           
        }    
    }

    void HandlePowerUps()
    {
        for (int i = 0; i < GameControl.control.m_ChosenPowerUps.Count; i++)
        {
            GameControl.control.m_Inventory.Remove(GameControl.control.m_ChosenPowerUps[i]);
            switch (GameControl.control.m_ChosenPowerUps[i])
            {
                case POWERUP.BALL:
                    AddPowerUp(m_ballBrickPrefab);                  
                    break;
                case POWERUP.BOMB:
                    AddPowerUp(m_bombBrickPrefab);
                    break;
                case POWERUP.SHOOT:
                    AddPowerUp(m_shootBrickPrefab);
                    break;
                case POWERUP.LIFE:
                    AddLife();
                    break;
            }        
        }
        GameControl.control.m_ChosenPowerUps.Clear();
        GameControl.control.Save();
    }

    void AddLife()
    {
        GameControl.control.m_LevelInfo.lives++;
    }

    void AddPowerUp(GameObject _ballBrickPrefab)
    {       
        GameObject brickToReplace = m_standardBricks[UnityEngine.Random.Range(0, m_standardBricks.Length)].gameObject;
        brickToReplace.gameObject.SetActive(false);
        GameObject GO= Instantiate(_ballBrickPrefab, brickToReplace.transform.position, Quaternion.identity);
        GO.transform.SetParent(m_LevelParenTGO.transform, true);              
    }

    public LevelOrder GetLevelOrder(string _tag)
    {
        for (int i = 0; i < m_levelOrder.Count; i++)
        {
            if (m_levelOrder[i].Tag == _tag)
            {
                return m_levelOrder[i];
            }
        }
        return null;
    }

    public void Win()
    {       
        if(m_GameState == GAMESTATE.PLAY || m_GameState == GAMESTATE.BOOST)
        {
            StartCoroutine(WinCO());           
        }
        m_powerupsManager.m_ResumeButton.gameObject.SetActive(false);
    }

    IEnumerator WinCO()
    {
        SendEndLevelDataToEvent(true);
        m_GameState = GAMESTATE.WIN;
        for (int i = 0; i < m_BallsList.Count; i++)
        {
            m_BallsList[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1.6f);
        while (GameControl.FindObjectsOfType<PointsMesh>().Length > 0 || m_isCheckingGrade)
        {
            yield return new WaitForSeconds(1.6f);
        }
        
        m_winScreen.transform.parent.gameObject.SetActive(true);             
    }

    void SendEndLevelDataToEvent(bool _win)
    {
        int timeMoves = 0;
        int levelNumber = GameControl.control.m_LevelIndex;
        if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.MOVES)
        {
            timeMoves = m_scoreManager.m_Moves;
        }

        else if (GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.TIME)
        {
            timeMoves = Mathf.RoundToInt(m_scoreManager.m_Time);
        }
        if (_win)
        {
            //int numberOfStars = GameControl.control.GetStars(m_scoreManager.Score, )
            //CustomEventsManager.LevelCompleteEvent(levelNumber, timeMoves, ActiveLifes, m_scoreManager.Score);
        }
        else
        {
            CustomEventsManager.LevelOverEvent(levelNumber, timeMoves, ActiveLifes);
        }
       
    }

    public void GameOver()
    {
        SendEndLevelDataToEvent(false);
        if (m_GameState == GAMESTATE.PLAY)
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].gameObject.SetActive(false);
            }
            m_GameState = GAMESTATE.LOSE;
            m_loseScreen.transform.parent.gameObject.SetActive(true);
            m_loseScreen.InitializeLoseScreen(GameControl.control.m_LevelInfo.levelRestriction, ActiveLifes);
            Ball.PlayerDeath();
        }       
    }

    void TakeNextLife()
    {
        if (m_GameState != GAMESTATE.PLAY)
            return;
        ActiveLifes--;
        if(ActiveLifes > 0)
        {
            DeactiveteLifeImage(ActiveLifes - 1);
            MakeBall(1f, new Vector3(1f, 1.25f, 0f), GetRandomStartDirection());
            Ball.PlayerRestart();
        }
    }

    public void DeactiveteLifeImage(int _index)
    {
        m_livesImagesParentGO.transform.GetChild(_index).gameObject.SetActive(false);
    }

    public void MakeBall(float _time, Vector3 _pos, Vector2 _direction)
    {
        Ball ball =  Instantiate(m_ballPrefab, _pos, Quaternion.identity) as Ball;
        m_BallsList.Add(ball);
        ball.InitializeBall(_pos, _direction, _time, false);       
        ActiveBalls++;
    }

    public static Vector3 GetRandomStartDirection()
    {
        return new Vector3(Random.Range(-.5f, .5f), 1f, 0f).normalized;
        //return Vector3.up;
    }

    void SetLives()
    {
        ActiveLifes = GameControl.control.m_LevelInfo.lives;
        for (int i = 0; i < ActiveLifes - 1; i++)
        {
            m_livesImagesParentGO.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void CheckIfWon()
    {
        if(m_GameState != GAMESTATE.WIN)
        {
            m_livesManager.ConsumeLife();
        }
    }

    public void WinDebug(int stars)
    {
        m_scoreManager.m_Moves = 1;
        m_scoreManager.m_Time = 1;
        ActiveLifes = 1;
        switch (stars)
        {
            case 1:
                m_scoreManager.Score = GameControl.control.m_StarsValues.Star1 + 1;
                break;
            case 2:
                m_scoreManager.Score = GameControl.control.m_StarsValues.Star2 + 1;
                break;
            case 3:
                m_scoreManager.Score = GameControl.control.m_StarsValues.Star3 + 1;
                break;
            default:
                break;
        }       
        Win();
    }

    public void CheckWinConditions()
    {       
        if (m_LevelGoal == LEVEL_GOAL.POINTS)
        {          
            if (m_bricksToDestroy <= 0)
            {
                if (m_scoreManager.Score >= GameControl.control.m_LevelInfo.PointsToAchieve)
                {
                    Win();
                }
                else
                {
                    GameOver();
                }
            }         
        }
        else if(m_LevelGoal == LEVEL_GOAL.ORDER)
        {
            for (int i = 0; i < m_levelOrder.Count; i++)
            {
                if (m_levelOrder[i].count > 0)
                    return;
            }
            Win();
        }
    }

    public void CheckBonus(int _hits)
    {
        if (!m_isCheckingGrade)
        {
            m_isCheckingGrade = true;
            StartCoroutine(CheckBonusCO(_hits));
        }
    }

    public IEnumerator CheckBonusCO(int _hits)
    {     
        yield return new WaitForSeconds(1.5f);
       
        if (_hits >= Ball.ContinousHits)
        {
            m_gradeAnimation.PlayAnimation(_hits);
            m_isCheckingGrade = false;
            Ball.ContinousHits = 0;
        }

        else
        {         
            StartCoroutine(CheckBonusCO(Ball.ContinousHits));
        }
    }

    void OnApplicationQuit()
    {
        CheckIfWon();
    }
}
