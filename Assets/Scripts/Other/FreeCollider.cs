using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCollider : MonoBehaviour {

    GameManager m_gameManager;

    Animator m_anim;

    AudioSource m_audio;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_anim = GetComponent<Animator>();
        m_audio = GetComponent<AudioSource>();
    }

     void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.CompareTag("Ball"))
        {
            m_anim.SetTrigger("Press_t");    
             m_gameManager.Win();
            m_audio.Play();
            _other.gameObject.SetActive(false);           
        }
    }
}
