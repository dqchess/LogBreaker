using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;

public class LoseScreen : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Text m_levelNumText;

    [SerializeField]
    Text m_headlineText;

    [SerializeField]
    Text m_countText;

    [SerializeField]
    Image m_itemImage;

    [SerializeField]
    Sprite m_movesSprite;

    [SerializeField]
    Sprite m_livesSprite;

    [SerializeField]
    Sprite m_timeSprite;

    GameManager m_gameManager;

    PopupOpener m_popupOpener;

    [Header("Variables")]

    [SerializeField]
    public int m_price;

    void Awake()
    {
        GetReferences();
        m_levelNumText.text = GameControl.control.m_ActiveLevelNum.ToString();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void InitializeLoseScreen(LevelRestriction _levelRestriction, int _activeLives)
    {
        if(_activeLives <= 0)
        {
            m_itemImage.sprite = m_livesSprite;
            m_headlineText.text = "Get additional life!";
            m_countText.text = "1x";
        }
        else if(_levelRestriction == LevelRestriction.MOVES)
        {
            m_itemImage.sprite = m_movesSprite;
            m_headlineText.text = "Get additional moves";
            m_countText.text = "15x";
        }
        else if (_levelRestriction == LevelRestriction.TIME)
        {
            m_itemImage.sprite = m_timeSprite;
            m_headlineText.text = "Get additional time";
            m_countText.text = "30x";
        }      
    }

    public void BuyItem()
    {
        if (GameControl.control.m_Coins >= m_price)
        {
            GameControl.control.m_Coins -= m_price;
            GameControl.control.Save();
            m_gameManager.HandlePurchase();
            FindObjectOfType<ScoreCounter>().UpdateText();
            CustomEventsManager.BoughtGameOverBoosterEvent();
        }
        else
        {
            m_popupOpener.OpenPopup();
        }     
    }
}
