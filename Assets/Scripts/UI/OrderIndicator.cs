using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderIndicator : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Image m_image;

    [SerializeField]
    Text m_amountLeftText;

    [SerializeField]
    Text m_amountTotalText;

    string m_name;

    int m_totalCount;

    int m_leftCount;
   

    public void Initialize(Sprite _sprite, int _count, string _name)
    {
        this.m_name = name;
        m_image.sprite = _sprite;
        m_amountLeftText.text = _count.ToString();
        m_amountTotalText.text = _count.ToString();
        m_totalCount = _count;
        m_leftCount = _count;

    }

    public void UpdateInidicatot()
    {

        m_leftCount--;
        m_amountLeftText.text = m_leftCount.ToString();
    }

    public void UpdateTotalAmount()
    {
        m_leftCount++;
        m_totalCount++;
        m_amountLeftText.text = m_leftCount.ToString();
        m_amountTotalText.text = m_totalCount.ToString();
    }
}
