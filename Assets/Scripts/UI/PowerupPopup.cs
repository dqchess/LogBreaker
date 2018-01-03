using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExaGames.Common;
using Ricimi;
using UnityEngine.SceneManagement;

public class PowerupPopup : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Text m_headlineText;

    [SerializeField]
    Text m_priceText;

    PowerUpsManager m_powerupsManager;

    GameManager m_gameManager;

    Popup m_popup;

    POWERUP m_powerup;

    int m_price;

    private void Awake()
    {
        m_popup = GetComponent<Popup>();
        m_powerupsManager = FindObjectOfType<PowerUpsManager>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void Initialize(string name, int price, POWERUP powerup)
    {

        m_headlineText.text = name;
        m_priceText.text = price.ToString();
        m_powerup = powerup;
        m_price = price;
    }

    public void BuyItem()
    {
        UpdateCoins();
        if (m_gameManager)
        {
            m_gameManager.m_LevelParenTGO.SetActive(true);
        }
        
        if (m_powerup == POWERUP.LIFE_NORMAL)
        {
            LivesManager livesManager = FindObjectOfType<LivesManager>();

            livesManager.GiveOneLife();          
        }
        else if (m_powerup == POWERUP.FULL_LIVES)
        {

            LivesManager livesManager = FindObjectOfType<LivesManager>();
            livesManager.FillLives();
        }


        else if (SceneManager.GetActiveScene().name != "Gameplay")
        {
            FindObjectOfType<PowerUpShop>().UpdateTexts();
        }

        
           
        else if((m_powerup == POWERUP.BOMB_BOOSTER || m_powerup == POWERUP.DESTROY_BOOSTER) && SceneManager.GetActiveScene().name == "Gameplay")
        {
            m_powerupsManager.UpdateBoosters();
            m_powerupsManager.SelectBooster(m_powerup);        
        }
        CustomEventsManager.BoughtShopBoosterEvent(m_powerup);

        

    }

    private void UpdateCoins()
    {
        GameControl.control.m_Inventory.Add(m_powerup);
        GameControl.control.m_Coins -= m_price;
        GameControl.control.Save();
        FindObjectOfType<ScoreCounter>().UpdateText();
    }

    public void Close()
    {
        
        m_popup.Close();
       
    }
}
