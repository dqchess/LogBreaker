using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

    Text m_amountText;

    int m_coins;

    Animator m_textAnim;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_amountText = GetComponent<Text>();
        m_textAnim = GetComponent<Animator>();
    }

    void Start()
    {
        UpdateValue();
    }

    public void UpdateText()
    {
        m_textAnim.Play("Update", -1, 0f);      
    }

    public void UpdateValue()
    {
        m_coins = GameControl.control.m_Coins;
        m_amountText.text = GameControl.control.m_Coins.ToString();
    }
}
