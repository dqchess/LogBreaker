using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class QuitPopup : MonoBehaviour {

    GameManager m_gameManager;

    PowerUpsManager m_powerupsManager;

    SceneTransition m_sceneTransition;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_powerupsManager = FindObjectOfType<PowerUpsManager>();
        m_sceneTransition = GetComponent<SceneTransition>();
    }

    public void Quit()
    {
        m_gameManager.CheckIfWon();
        m_sceneTransition.PerformTransition();
    }

    public void Resume()
    {
        m_powerupsManager.ResumeBalls();
    }
}
