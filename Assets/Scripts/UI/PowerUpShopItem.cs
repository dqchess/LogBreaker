using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;
using ExaGames.Common;

public class PowerUpShopItem : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Sprite ItemSprite;

    [SerializeField]
    GameObject PowerupPopup;

    [SerializeField]
    GameObject CoinsShopPopup;

    [SerializeField]
    GameObject FullLivesPopup;

    [SerializeField]
    GameObject InfoPopup;

    PopupOpener m_popupOpener;

    Text m_priceText;

    LivesManager m_livesManager;

    [Header("Variables")]

    [SerializeField]
    POWERUP powerUp;

    [SerializeField]
    int Price;

    [TextArea(2, 10)]
    public string description;

    void Awake()
    {
        GetReferences();
        m_priceText.text = Price.ToString();
    }

    void GetReferences()
    {
        m_priceText = GetComponentInChildren<Text>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void BuyItem()
    {
        if (powerUp == POWERUP.LIFE_NORMAL || powerUp == POWERUP.FULL_LIVES)
        {
            LivesManager livesManager = FindObjectOfType<LivesManager>();
            if (livesManager.HasMaxLives)
            {
                m_popupOpener.popupPrefab = FullLivesPopup;
                m_popupOpener.OpenPopup();
                return;               
            }
        }

        if (GameControl.control.m_Coins >= Price)
        {         
            m_popupOpener.popupPrefab = PowerupPopup;
            m_popupOpener.OpenPopup();
            FindObjectOfType<PowerupPopup>().Initialize(gameObject.name, Price, powerUp);
        }
        else
        {
            m_popupOpener.popupPrefab = CoinsShopPopup;
            m_popupOpener.OpenPopup();
        }
        
    }

    public void ShowInfo()
    {
        m_popupOpener.popupPrefab = InfoPopup;
        m_popupOpener.OpenPopup();
        FindObjectOfType<PowerUpInfoPopup>().Initialize(gameObject.name, description, ItemSprite);
    }
 
}
