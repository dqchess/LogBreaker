using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class GameplayPopupOpener : MonoBehaviour {

    PopupOpener m_popupOpener;

    GameManager m_gameManager;

    void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void Open()
    {
        if(m_gameManager.m_GameState != GAMESTATE.INIT)
        {
            m_popupOpener.OpenPopup();
        }
    }
}
