using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GameObject m_pauseGO;

    [SerializeField]
    Text m_countdownText;

    [SerializeField]
    GameObject m_buttonGO;

    PowerUpsManager m_powerUpsManager;

    [Header("Variables")]

    [SerializeField]
    int m_countTime;


    void Awake()
    {
        m_powerUpsManager = FindObjectOfType<PowerUpsManager>();      
    }

    public void Initialize()
    {
        m_pauseGO.SetActive(true);
        m_buttonGO.SetActive(true);       
    }

    void DeactivateButtons()
    {
        m_pauseGO.SetActive(false);
        m_buttonGO.SetActive(false);
    }

    public void StartCountdown(bool _count)
    {
        m_countdownText.gameObject.SetActive(true);
        DeactivateButtons();
        StartCoroutine(StartCountdownCO(_count));
    }

    IEnumerator StartCountdownCO(bool _count)
    {
        if (_count)
        {
            for (int i = 0; i < m_countTime; i++)
            {
                m_countdownText.text = (m_countTime - i).ToString();

                yield return new WaitForSeconds(1f);
            }
        }
       
        m_countdownText.text = "GO!";
        yield return new WaitForSeconds(.5f);
        m_countdownText.gameObject.SetActive(false);
        m_powerUpsManager.StartBalls();
    }
}
