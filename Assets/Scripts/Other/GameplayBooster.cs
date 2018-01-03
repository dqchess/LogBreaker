using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;


public class GameplayBooster : MonoBehaviour {

    [Header("References")]

    public Image m_BGimage;

    [SerializeField]
    Image m_plusImage;

    [SerializeField]
    Text m_amountText;

    [SerializeField]
    GameObject m_coinsShopPopup;

    [SerializeField]
    GameObject m_buyPopup;

    PowerUpsManager m_powerupManager;

    PopupOpener m_popupOpener;

    GameManager m_gameManager;

    [Header("Variables")]

    public POWERUP m_PowerUp;

    public Color m_EnabledColor;

    public Color m_DisabledColor;
    
    [SerializeField]
    public int m_price;

    [HideInInspector]
    public bool m_IsSelected;

    [HideInInspector]
    public int m_Amount;
 
    void Awake()
    {
        GetReferences();
        m_BGimage.color = m_DisabledColor;
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_popupOpener = GetComponent<PopupOpener>();
        m_powerupManager = FindObjectOfType<PowerUpsManager>();
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_Amount = 0;
        List<POWERUP> inventory = GameControl.control.m_Inventory;
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i] == m_PowerUp)
            {
                m_Amount++;
            }
        }
        if(m_Amount == 0)
        {
            m_plusImage.gameObject.SetActive(true);
            m_amountText.gameObject.SetActive(false);
        }
        else
        {
            m_plusImage.gameObject.SetActive(false);
            m_amountText.gameObject.SetActive(true);
            m_amountText.text = "x" + m_Amount;
        }
    }

    public void Select()
    {
        if(m_gameManager.m_GameState != GAMESTATE.PLAY && m_gameManager.m_GameState != GAMESTATE.BOOST)
        {
            return;
        }
        if(m_Amount == 0)
        {
           
            m_powerupManager.StopBalls();
            if(GameControl.control.m_Coins >= m_price)
            {
                m_popupOpener.popupPrefab = m_buyPopup;
                m_popupOpener.OpenPopup();
                FindObjectOfType<PowerupPopup>().Initialize(gameObject.name, m_price, m_PowerUp);
            }
            else
            {
                m_popupOpener.popupPrefab = m_coinsShopPopup;
                m_popupOpener.OpenPopup();
            }
        }
        
        else
        {
            if (!m_IsSelected)
            {
                m_powerupManager.Select(this);
            }
            else
            {
                m_powerupManager.Deselct(this);
            }
        }       
    }

    void Deselect()
    {
        m_BGimage.color = m_DisabledColor;
    }

  
}
