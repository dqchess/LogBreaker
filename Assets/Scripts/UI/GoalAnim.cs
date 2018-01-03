using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalAnim : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Image[] m_images;

    [SerializeField]
    Text m_descriptionText;

    GameManager m_gameManager;

    void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void StartGame()
    {
        m_gameManager.LaunchBallAtStart();
    }

    public void Initialize(GoalDescription _goalDescription)
    {
        for (int i = 0; i < _goalDescription.Sprites.Length; i++)
        {
            m_images[i].gameObject.SetActive(true);
            m_images[i].sprite = _goalDescription.Sprites[i];
        }
        m_descriptionText.text = _goalDescription.Description;
    }
}
