using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum TRAP_SIDE
{
    TOP,
    LEFT,
    RIGHT
}

public class LoseTrap : LoseCollider {

    [SerializeField]
    public TRAP_SIDE trapSide;

    CircleCollider2D m_collider2D;

    ParticleSystem m_ballExplosion;

    Animator m_anim;

    GameManager m_gameManager;

    protected override void Awake()
    {
        base.Awake();
        GetReferences();
    }

    void GetReferences()
    {
        m_ballExplosion = GetComponentInChildren<ParticleSystem>();
        m_collider2D = GetComponent<CircleCollider2D>();
        m_anim = GetComponent<Animator>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        Ball.OnPlayerDeath += StopTrap;
        Ball.OnPlayerRestart += StartTrap;
        StartCoroutine(StartTrapCO(12f));
    }

    void StartTrap()
    {
        StartCoroutine(StartTrapCO(6f));
    }

    IEnumerator StartTrapCO(float _time)
    {       
        yield return new WaitForSeconds(_time);      
        m_anim.SetBool("IsStopped_b", false);              
        m_collider2D.isTrigger = true;       
    }

    void StopTrap()
    {
        if (m_gameManager.ActiveBalls > 1)
            return;
        StartCoroutine(StopTrapCO(1.5f));
    }

    IEnumerator StopTrapCO(float _time)
    {       
        yield return new WaitForSeconds(_time);
        m_collider2D.isTrigger = false;
        m_anim.SetBool("IsStopped_b", true);
    }

    protected override void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other);
        if (!_other.CompareTag("Ball"))
            return;             
        
        m_ballExplosion.transform.position = _other.transform.position;
        ParticleSystem.VelocityOverLifetimeModule explosionVelocity = m_ballExplosion.velocityOverLifetime;
        ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve();
        switch (trapSide)
        {
            case TRAP_SIDE.TOP:
                rate.constantMax = -5.0f;
                explosionVelocity.x = rate;
                break;
            case TRAP_SIDE.LEFT:
                rate.constantMax = 5.0f;
                explosionVelocity.x = rate;
                break;
            case TRAP_SIDE.RIGHT:
                rate.constantMax = -5.0f;
                explosionVelocity.x = rate;
                break;
            default:
                break;
        }
  
        explosionVelocity.x = rate;
        m_ballExplosion.GetComponent<ParticleSystemRenderer>().material = _other.GetComponent<Renderer>().material;
        m_ballExplosion.Play();       
    }
}
