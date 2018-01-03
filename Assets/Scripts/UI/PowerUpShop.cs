using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using UnityEngine.UI;
using System;

public class PowerUpShop : MonoBehaviour {

    [Header("References")]
    [Space(10)]

    [Header("Texts")]
    [SerializeField]
    Text m_bombCountText;

    [SerializeField]
    Text m_ballCountText;

    [SerializeField]
    Text m_shootCountText;

    [SerializeField]
    Text m_lifesCountText;

    [SerializeField]
    Text m_destroyCountText;

    [SerializeField]
    Text m_bombGameplayCountText;

    [Header("Toggles")]

    [SerializeField]
    Toggle m_bombToggle;

    [SerializeField]
    Toggle m_ballToggle;

    [SerializeField]
    Toggle m_shootToggle;

    [SerializeField]
    Toggle m_lifeToggle;

    int m_ballCount;

    int m_bombCount;

    int m_shootCount;

    int m_lifesCount;

    int m_destroyCount;

    int m_bombGameplayCount;


    void Awake()
    {
        UpdateTexts();
        GameControl.control.m_ChosenPowerUps.Clear();
    }

    public void UpdateTexts()
    {
        m_ballCount = 0;
        m_lifesCount = 0;
        m_shootCount = 0;
        m_bombCount = 0;
        m_destroyCount = 0;
        m_bombGameplayCount = 0;
        for (int i = 0; i < GameControl.control.m_Inventory.Count; i++)
        {
            switch (GameControl.control.m_Inventory[i])
            {
                case POWERUP.BALL:
                    m_ballCount++;
                    break;
                case POWERUP.BOMB:
                    m_bombCount++;
                    break;
                case POWERUP.SHOOT:
                    m_shootCount++;
                    break;
                case POWERUP.LIFE:
                    m_lifesCount++;
                    break;
                case POWERUP.DESTROY_BOOSTER:
                    m_destroyCount++;
                    break;
                case POWERUP.BOMB_BOOSTER:
                    m_bombGameplayCount++;
                    break;
            }
        }    
        SetToggleAndText(m_ballCountText, m_ballCount, m_ballToggle);
        SetToggleAndText(m_shootCountText, m_shootCount, m_shootToggle);
        SetToggleAndText(m_lifesCountText, m_lifesCount, m_lifeToggle);
        SetToggleAndText(m_bombCountText, m_bombCount, m_bombToggle);
        m_destroyCountText.text = "x" + m_destroyCount.ToString();
        m_bombGameplayCountText.text = "x" + m_bombGameplayCount.ToString();
    }

    void SetToggleAndText(Text _text, int _count, Toggle _toggle)
    {
        if(_count == 0)
        {
            _toggle.gameObject.SetActive(false);          
        }

        else
        {
            _toggle.gameObject.SetActive(true);        
        }
        _text.text = "x" + _count.ToString();
    }

    public void SetChosenPowerUps()
    {
        if (m_ballToggle.isOn && m_ballToggle.IsActive())
        {
            GameControl.control.m_ChosenPowerUps.Add(POWERUP.BALL);
        }
        if (m_bombToggle.isOn && m_bombToggle.IsActive())
        {
            GameControl.control.m_ChosenPowerUps.Add(POWERUP.BOMB);
        }
        if (m_shootToggle.isOn && m_shootToggle.IsActive())
        {
            GameControl.control.m_ChosenPowerUps.Add(POWERUP.SHOOT);
        }
        if (m_lifeToggle.isOn && m_lifeToggle.IsActive())
        {
            GameControl.control.m_ChosenPowerUps.Add(POWERUP.LIFE);
        }

    }
}
