using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCounter : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    OrderIndicator[] m_orderIndicators;

    [SerializeField]
    Sprite[] m_sprites;

    GameManager m_gameManager;

    void Awake()
    {      
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void SetOrder(List<LevelOrder> orders)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            SetIndicator(i, orders[i], orders[i].Tag);
        }
    }

    void SetIndicator(int _i, LevelOrder _levelOrder, string _name)
    {
        m_orderIndicators[_i].name = name;
        m_orderIndicators[_i].gameObject.SetActive(true);
        Sprite sprite = GetSprite(_levelOrder.Tag);
        int count = _levelOrder.count;
        m_orderIndicators[_i].Initialize(sprite, count, name);   
    }

    public Sprite GetSprite(string _tag)
    {
        for (int i = 0; i < m_sprites.Length; i++)
        {
            if(m_sprites[i].name == _tag)
            {
                return m_sprites[i];
            }
        }
        return null;
    }

    public void UpdateCounter(string _tag)
    {    
        GetIndicator(_tag).UpdateInidicatot();  
    }

    public void UpdateTotal(string _tag)
    {
        GetIndicator(_tag).UpdateTotalAmount();
    }

    OrderIndicator GetIndicator(string _tag)
    {
        for (int i = 0; i < m_orderIndicators.Length; i++)
        {
            if (m_orderIndicators[i].name == _tag)
            {
                return m_orderIndicators[i];
            }
        }
        return null;
    }

}
