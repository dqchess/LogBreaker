using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Ricimi;
using ExaGames.Common;
using UnityEngine.PostProcessing;

[System.Serializable]
public class StarsValues
{
    public int Star1;
    public int Star2;
    public int Star3;
}

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    [Header("References")]

    public LevelsStarValuesSO m_LevelsStarValuesSO;
    
    public Sprite[] Backgrounds;

    [HideInInspector]
    public GoalDescription m_GoalDescription;

    [HideInInspector]
    public StarsValues m_StarsValues;

    [Header("Variables")]

    [SerializeField]
    int m_startingCoins;

    [HideInInspector]
    public int m_ActiveWorldNum;

    [HideInInspector]
    public int m_ActiveLevelNum;

    [HideInInspector]
    public int m_ActiveLevelScore;

    [HideInInspector]
    public int m_ActiveLevelStars;

    [HideInInspector]
    public List<POWERUP> m_ChosenPowerUps;
    
    [Header("Player Data")]

    [HideInInspector]
    public List<WorldState> m_WorldStates;

    [HideInInspector]
    public int m_Coins;

    [HideInInspector]
    public int m_AvaliableWorldsCount;

    [HideInInspector]
    public List<POWERUP> m_Inventory;

    [HideInInspector]
    public int m_CointToAdd;

    [HideInInspector]
    public LevelInfo m_LevelInfo;

    [HideInInspector]
    public int m_LevelIndex;

    [HideInInspector]
    public bool m_IsLoggedIn;

    void Awake()
    {
        MakePersistentSingleton();
        if (!PlayerPrefs.HasKey("world"))
        {
            PlayerPrefs.SetInt("world", 0);
        }
        m_ChosenPowerUps = new List<POWERUP>();
    }

    void MakePersistentSingleton()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Application.persistentDataPath + "/playerInfo.dat" != null)
            Load();             
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Levels"))
        {
            CheckNewWorlds();
            Ball.ContinousHits = 0;
        }       
    }

    void CheckNewWorlds()
    {
        Toggle[] worlds = GameObject.Find("Toggles").GetComponentsInChildren<Toggle>();
        int newWorldsCount = (worlds.Length - m_AvaliableWorldsCount);

        if(newWorldsCount > 0)
        {
            for (int i = 0; i < newWorldsCount; i++)
            {
                AddWorld();
            }
        }
        m_AvaliableWorldsCount = worlds.Length;
        FindObjectOfType<CategoriesMover>().m_NumOfPages = worlds.Length; 
    }

    public void CloseAllPopups()
    {
        Popup[] popups = FindObjectsOfType<Popup>();
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].Close();
        }
    }

    public void SetBestScore(int _score)
    {
        WorldState worldState = GetWorldState(m_ActiveWorldNum - 1);

        if (_score > worldState.scores[m_ActiveLevelNum - 1])
        {
            worldState.scores[m_ActiveLevelNum - 1] = _score;
            GPSManager.SetRecoredForLeaderboard(worldState);
            GPSManager.CheckForAchievments(worldState);
            //Debug.Log("Set best score");
        }

        if(m_ActiveLevelNum != 10 && worldState.scores[m_ActiveLevelNum] == 0)
        {
            //Debug.Log("Unlock level");
            worldState.scores[m_ActiveLevelNum] = 1;        
        }

        else if(m_ActiveLevelNum == 10 && m_ActiveWorldNum < m_AvaliableWorldsCount)
        {
            //Debug.Log("Unlock world");
            GetWorldState(m_ActiveWorldNum).scores[0] = 1;
        }
        else if (m_ActiveLevelNum == 10 && m_ActiveWorldNum == m_AvaliableWorldsCount)
        {
            //Debug.Log("last level");
        }

        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath
            + "/playerInfo.dat", FileMode.Create);
        PlayerData playerData = new PlayerData();      
        playerData.worldStatesList = m_WorldStates;
        playerData.Coins = m_Coins;
        playerData.activeWorldsCount = m_AvaliableWorldsCount;
        playerData.inventory = m_Inventory;
        bf.Serialize(file, playerData);
        file.Close();
    }
   
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath
                + "/playerInfo.dat", FileMode.Open);
            PlayerData playerData = (PlayerData)bf.Deserialize(file);
            m_WorldStates = playerData.worldStatesList;
            m_Coins = playerData.Coins;
            m_AvaliableWorldsCount = playerData.activeWorldsCount;
            m_Inventory = playerData.inventory;
            file.Close();        
        }

        else
        {
           // Debug.Log("New data on load");
            CreatePlayerData();
        }
    }

    void CreatePlayerData()
    {
        m_WorldStates = new List<WorldState>();
        m_Coins = m_startingCoins;
        m_AvaliableWorldsCount = 3;
        m_Inventory = new List<POWERUP>();
        for (int i = 0; i < m_AvaliableWorldsCount; i++)
        {
            int[] scores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            WorldState worldState = new WorldState(scores, i);
            m_WorldStates.Add(worldState);
        }             
        Save();
    }

    void AddWorld()
    {
        //Debug.Log("Adding World");     
        int[] scores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        if(m_WorldStates[m_AvaliableWorldsCount - 1].scores[9] != 0)
        {
            scores[0] = 1;
        }
        WorldState worldState = new WorldState(scores, m_WorldStates.Count);
        m_WorldStates.Add(worldState);
        Save();
    }

    public WorldState GetWorldState(int _worldNum)
    {
        //Debug.Log(worldNum);
        for (int i = 0; i < m_WorldStates.Count; i++)
        {
            if(m_WorldStates[i].worldNum == _worldNum)
            {

                return m_WorldStates[i];
            }
        }
        return null;
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        Caching.ClearCache();
        File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        FindObjectOfType<LivesManager>().FillLives();
        CreatePlayerData();
    }

    public int GetScore(int _worldNum, int _levelNu)
    {
        int score = m_WorldStates[_worldNum - 1].scores[_levelNu - 1];

        return score;
    }

    public int GetStars(int _score, StarsValues _starValues)
    {
        int stars = 0;
        if (_score < _starValues.Star1)
        {
            stars = 0;
        }
        else if (_score < _starValues.Star2)
        {
            stars = 1;
        }
        else if (_score < _starValues.Star3)
        {
            stars = 2;
        }
        else if (_score >= _starValues.Star3)
        {
            stars = 3;
        }
        return stars;


    }

    public void AdCoins(int _coinsToAdd)
    {
        m_CointToAdd = _coinsToAdd;
        m_Coins += _coinsToAdd;
        Save();
    }

    public int GetLastWorld()
    {
        for (int i = 0; i < m_WorldStates.Count; i++)
        {
            for (int j = 0; j < m_WorldStates[i].scores.Length; j++)
            {
                if(m_WorldStates[i].scores[j] == 1)
                {
                    Debug.Log(i);
                    return i;
                }
            }
        }
        return 11;
        
    }
}
