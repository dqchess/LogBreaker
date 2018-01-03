using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinShopItem : MonoBehaviour {

    [SerializeField]
    int m_coins;

    [SerializeField]
    Text m_priceText;

    void Awake()
    {
        m_priceText.text = m_coins.ToString();
    }

    public void BuyCoins()
    {
        if (AdManager.Instance.m_IsPlayingAd)
            return;
        GameControl.control.m_CointToAdd = m_coins;
        AdManager.Instance.m_AdReward = AD_REWARD.COINS;
        AdManager.Instance.ShowAd();
    }
}
