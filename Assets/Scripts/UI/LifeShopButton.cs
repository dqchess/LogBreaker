using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExaGames.Common;
using Ricimi;

public class LifeShopButton : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    GameObject LifeShopPopup;

    [SerializeField]
    GameObject FullLivesPopup;

    LivesManager m_livesManager;

    PopupOpener m_popupOpener;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_popupOpener = GetComponent<PopupOpener>();
        m_livesManager = GetComponentInParent<LivesManager>();
    }

    public void OpenPopup()
    {
        if (!m_livesManager.HasMaxLives)
        {
            m_popupOpener.popupPrefab = LifeShopPopup;
            m_popupOpener.OpenPopup();
        }
        else
        {
            m_popupOpener.popupPrefab = FullLivesPopup;
            m_popupOpener.OpenPopup();
        }
    }
}
