using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brick : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    public PointsMesh pointsMeshPrefab;

    protected PlayerPowerup m_playerPowerup;

    protected GameManager m_gameManager;

    BoxCollider2D m_boxCollider2D;

    ScoreManager m_scoreManager;

    [Header("Variables")]

    [SerializeField]
    int Points;

    [SerializeField]
    Color PointsColor;

    [SerializeField]
    float ShootDamage;

    [SerializeField]
    float m_timeToDestroyBrick;

    public float TimeToDestroyBrick
    {
        get
        {
            return m_timeToDestroyBrick;
        }
        set
        {
            m_timeToDestroyBrick = value;
            if (m_timeToDestroyBrick <= 0)
            {

                DestroyBrick();
            }
        }
    }

    [SerializeField]
    float m_hitPoints;

    protected bool m_wasDestroyed;

    public float HitPoints
    {
        get
        {
            return m_hitPoints;
        }
        set
        {
            m_hitPoints = value;
            if(m_hitPoints <= 0)
            {             
                DestroyBrick();              
            }
        }
    }

    protected virtual void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_playerPowerup = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerup>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
    }

    protected virtual void OnParticleCollision(GameObject _other)
    {
        if (m_gameManager.m_GameState != GAMESTATE.PLAY)
            return;
        if (gameObject.activeInHierarchy)
            HitPoints -= ShootDamage;
    }

    protected virtual void DestroyBrick(GameObject _collidingBall = null)
    {
        gameObject.SetActive(false);
        m_gameManager.CheckIfValidDestruction(gameObject.tag);
        if (Points != 0)
        {
            PointsMesh pointsMesh = Instantiate(pointsMeshPrefab, transform.position, Quaternion.identity);
            pointsMesh.Initialize(Points, PointsColor, true);
        }       
    }

    void OnCollisionExit2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Ball") || _collision.gameObject.CompareTag("Fireball"))
        {
            HandleBallCollision(true, _collision.gameObject);       
        }       
    }

    public virtual void HandleBallCollision(bool _isBallCollision, GameObject _collodingBall = null)
    {       
        if (_isBallCollision)
        {
            Ball.ContinousHits++;
        }
        
        if(Ball.ContinousHits >= 3)
        {
            m_gameManager.CheckBonus(Ball.ContinousHits);           
        }
        HitPoints--;      
    }

    public void PerformBoost()
    {
        PowerUpsManager powerupManager = FindObjectOfType<PowerUpsManager>();
        powerupManager.Perform(this);
    }
}
