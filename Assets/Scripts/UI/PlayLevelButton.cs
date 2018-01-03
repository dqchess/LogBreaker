using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ricimi;
using ExaGames.Common;

public class PlayLevelButton : MonoBehaviour {

    LivesManager m_lifesManager;

    SceneTransition m_sceneTransition;

    PopupOpener m_popupOpener;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_lifesManager = FindObjectOfType<LivesManager>();
        m_sceneTransition = GetComponent<SceneTransition>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void PlayLevel()
    {
        if (m_lifesManager.CanPlay)
        {         
            m_sceneTransition.scene = "Gameplay";
            m_sceneTransition.PerformTransition();
        }
        else
        {
            m_popupOpener.OpenPopup();
        }           
    }
}
