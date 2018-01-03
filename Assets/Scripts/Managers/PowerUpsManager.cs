using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//public enum GAMEPLAY_BOOSTER
//{
//    DESTROY_BRICK,
//    BOMB
//}

public class PowerUpsManager : MonoBehaviour {

    [Header("References")]

    public ResumeButton m_ResumeButton;

    [SerializeField]
    GameplayBooster[] m_boosters;

    [SerializeField]
    public Brick m_bombPrefab;


    Physics2DRaycaster m_raycaster2D;

    GameManager m_gameManager;

    PlayerMovement m_playerMovement;

    ParticleSystem m_thunderEffect;

    AudioSource m_thunderAudio;

    OrderCounter m_orderCounter;

    [Header("Variables")]

    [HideInInspector]
    public bool m_IsGamePaused;

    int m_selectedBoosterAmount;

    POWERUP m_gameplayPowerUp;

   
    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_thunderEffect = GetComponentInChildren<ParticleSystem>();
        m_thunderAudio = m_thunderEffect.GetComponent<AudioSource>();
        m_raycaster2D = Camera.main.GetComponent<Physics2DRaycaster>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_playerMovement = FindObjectOfType<PlayerMovement>();
        m_orderCounter = FindObjectOfType<OrderCounter>();
    }

    public void SwitchBoosters(bool on)
    {
        for (int i = 0; i < m_boosters.Length; i++)
        {
            m_boosters[i].gameObject.SetActive(on);
        }
    }

    public void Select(GameplayBooster _booster)
    {     
        for (int i = 0; i < m_boosters.Length; i++)
        {
            Deselct(m_boosters[i]);
        }

        m_gameplayPowerUp = _booster.m_PowerUp;
        _booster.m_BGimage.color = _booster.m_EnabledColor;
        _booster.m_IsSelected = true;
        m_selectedBoosterAmount = _booster.m_Amount;
        StopBalls();
    }

    public void Deselct(GameplayBooster _booster)
    {
        _booster.m_BGimage.color = _booster.m_DisabledColor;
        _booster.m_IsSelected = false;
        m_selectedBoosterAmount = 0;
    }

    public void SelectBooster(POWERUP _powerup)
    {
        for (int i = 0; i < m_boosters.Length; i++)
        {
            if(m_boosters[i].m_PowerUp == _powerup)
            {
                m_boosters[i].Select();
            }
        }
    }

    public void UpdateBoosters()
    {
        for (int i = 0; i < m_boosters.Length; i++)
        {
            m_boosters[i].Initialize();
        }
    }

    public void Perform(Brick _brick)
    {       
        if (m_selectedBoosterAmount <= 0)
            return;

        switch (m_gameplayPowerUp)
        {
            case POWERUP.DESTROY_BOOSTER:
                _brick.HitPoints = 0;
                m_thunderEffect.transform.position = _brick.transform.position;
                m_thunderAudio.Play();
                m_thunderEffect.Play();
                break;
            case POWERUP.BOMB_BOOSTER:
                _brick.HitPoints = 0;
                BrickBomb bombBrick =  Instantiate(m_bombPrefab, _brick.transform.position, Quaternion.identity) as BrickBomb;
                bombBrick.Explode();
                bombBrick.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        if (m_ResumeButton.isActiveAndEnabled)
        {
            m_ResumeButton.StartCountdown(false);
        }
        else
        {
            m_playerMovement.m_CanMove = true;
        }
        
        m_selectedBoosterAmount--;
        GameControl.control.m_Inventory.Remove(m_gameplayPowerUp);
        GameControl.control.Save();
        for (int i = 0; i < m_boosters.Length; i++)
        {
            Deselct(m_boosters[i]);
        }
        UpdateBoosters();
    }

    public void ResumeBalls()
    {       
        m_gameManager.m_GameState = GAMESTATE.INIT;
        SwitchBoosters(false);
        for (int i = 0; i < m_boosters.Length; i++)
        {
            Deselct(m_boosters[i]);
        }
        m_raycaster2D.enabled = false;       
        GameControl.control.CloseAllPopups();
        m_ResumeButton.StartCountdown(true);        
    }

    public void StartBalls()
    {
        m_IsGamePaused = false;
        m_gameManager.m_GameState = GAMESTATE.PLAY;
        for (int i = 0; i < m_gameManager.m_BallsList.Count; i++)
        {
            m_gameManager.m_BallsList[i].Resume();
        }
 
        m_ResumeButton.gameObject.SetActive(false);
        m_playerMovement.m_CanMove = true;
        
        SwitchBoosters(true);
    }

    public void StopBalls()
    {      
        Pause();
        m_raycaster2D.enabled = true;
    }

    public void Pause()
    {
        if (m_gameManager.m_GameState == GAMESTATE.INIT || 
            m_gameManager.m_GameState == GAMESTATE.WIN ||
            m_gameManager.m_GameState == GAMESTATE.LOSE)
            return;

        m_IsGamePaused = true;
        
        m_gameManager.m_GameState = GAMESTATE.BOOST;
        for (int i = 0; i < m_gameManager.m_BallsList.Count; i++)
        {
            m_gameManager.m_BallsList[i].Stop();
        }      
        m_playerMovement.m_CanMove = false;
        m_ResumeButton.gameObject.SetActive(true);
        m_ResumeButton.Initialize();
    }
}
