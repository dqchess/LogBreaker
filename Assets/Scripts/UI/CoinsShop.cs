using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using UnityEngine.SceneManagement;

public class CoinsShop : MonoBehaviour {

    PopupOpener m_popupOpener;
    
    void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void OpenShop()
    {
        if (FindObjectOfType<CoinShopItem>())
        {
            return;
        }
        else
        {
            m_popupOpener.OpenPopup();
        }
    }
}
