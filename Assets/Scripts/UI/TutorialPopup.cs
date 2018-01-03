using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour {

    GameManager m_gameManager;

    void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void Initializegame()
    {
        m_gameManager.Initialize();
    }
}
