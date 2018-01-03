using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Ricimi;
using UnityEngine.UI;
using ExaGames.Common;
using UnityEngine.SceneManagement;

public enum AD_REWARD
{
    DOUBLE_POINTS,
    LIVE,
    COINS
}

public class AdManager : MonoBehaviour
{ 
    public static AdManager Instance;

    [Header("References")]

    [SerializeField]
    GameObject m_skippedPopup;

    [SerializeField]
    GameObject m_failedPopup;

    PopupOpener m_popupOpener;

    PowerUpsManager m_powerUpsManager;

    GameManager m_gameManager;

    [HideInInspector]
    public AD_REWARD m_AdReward;

    [HideInInspector]
    public bool m_IsPlayingAd;

    void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
        MakePersistantSingleton();
    }

    void MakePersistantSingleton()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Advertisement.Initialize("1392021", false);     
    }

    void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name == "Gameplay")
        {
            GetReferences();
        }
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    public void ShowAd()
    {
        if (m_IsPlayingAd)
            return;
        m_IsPlayingAd = true;
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhandler;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //m_popupOpener.popupPrefab = FailedAdPopup;
            //m_popupOpener.OpenPopup();
            return;
        }
        float startTime = Time.timeSinceLevelLoad;
       
        if (!Advertisement.isInitialized)
        {

            Advertisement.Initialize(Advertisement.gameId);
            while (!Advertisement.isInitialized)
            {
                if (Time.timeSinceLevelLoad - startTime > 10f)
                    break;
            }

        }
        startTime = Time.timeSinceLevelLoad;

        while (!Advertisement.IsReady())
        {
            if (Time.timeSinceLevelLoad - startTime > 10f)
                break;
        }
        Advertisement.Show(options);

    }

    void AdCallbackhandler(ShowResult _result)
    {
        m_IsPlayingAd = false;
        switch (_result)
        {
            case ShowResult.Finished:
                switch (m_AdReward)
                {
                    case AD_REWARD.DOUBLE_POINTS:
                        FindObjectOfType<WinScreen>().RewardDoublePoints();                    
                        break;
                    case AD_REWARD.COINS:
                        GameControl.control.m_Coins += GameControl.control.m_CointToAdd;
                        GameControl.control.Save();
                        ScoreCounter scoreCounter = FindObjectOfType<ScoreCounter>();
                        scoreCounter.UpdateText();
                        
                        if (SceneManager.GetActiveScene().buildIndex > 1)
                        {
                            GameControl.control.CloseAllPopups();
                            m_gameManager.m_LevelParenTGO.SetActive(true);                        
                        }
                        break;
                }    
                break;
            case ShowResult.Skipped:
               // m_popupOpener.popupPrefab = SkippedAdPopup;
               // m_popupOpener.OpenPopup();
                break;

            case ShowResult.Failed:
               // m_popupOpener.popupPrefab = FailedAdPopup;
               // m_popupOpener.OpenPopup();
                break;
        }
    }
}
