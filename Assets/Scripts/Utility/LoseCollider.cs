using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCollider : MonoBehaviour {

    GameManager m_gameManager;

    AudioSource m_audio;

    protected virtual void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_audio = GetComponent<AudioSource>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Ball"))
        {          
            Ball.PlayerDeath();
            _other.gameObject.SetActive(false);
            m_gameManager.ActiveBalls--;
            m_gameManager.m_BallsList.Remove(_other.GetComponent<Ball>());
            m_audio.Play();
            Ball.ContinousHits = 0;
        }
    }
}
