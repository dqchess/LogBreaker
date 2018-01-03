using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour {

    GameManager m_gameManager;

    void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            m_gameManager.MakeBall(0f, transform.position, Vector3.up);
            gameObject.SetActive(false);
        }
    }
}
