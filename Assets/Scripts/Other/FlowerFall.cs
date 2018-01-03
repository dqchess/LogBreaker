using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFall : MonoBehaviour {

    float m_shootDuration;

    PlayerPowerup m_playerPowerup;

    protected virtual void Awake()
    {
        m_playerPowerup = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerup>();
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            m_playerPowerup.GetComponentInChildren<PlayerShooting>().Shoot(m_shootDuration);
            gameObject.SetActive(false);
        }
    }

    public void Initialize(float _shootDuration)
    {
        m_shootDuration = _shootDuration;
    }
}
