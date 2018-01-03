using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {

    public delegate void PlayerDeathHandler();

    public static event PlayerDeathHandler OnPlayerDeath;

    public static event PlayerDeathHandler OnPlayerRestart;

    public static int ContinousHits;

    [Header("References")]

    [SerializeField]
    AudioClip m_borderClip;

    [SerializeField]
    AudioClip m_undestructableClip;

    [SerializeField]
    AudioClip m_paddleClip;

    [SerializeField]
    AudioClip m_startClip;

    AudioSource m_audio;

    GameManager m_gameManager;

    PowerUpsManager m_powerUpManager;

    ScoreManager m_scoreManager;

    Transform m_player;

    [Header("Variables")]

    public float m_bounceOffser;

    [SerializeField]
    Vector2 m_levelBorders; 

    [SerializeField]
    float m_speed;

    [SerializeField]
    public Vector3 m_startingPos;

    Vector3 m_direction;

    bool m_isStopped;

    Vector2 m_velocity;

    Rigidbody2D m_RB;
    
    protected virtual void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_powerUpManager = FindObjectOfType<PowerUpsManager>();
        m_RB = GetComponent<Rigidbody2D>();
        m_audio = GetComponent<AudioSource>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void RestetOnPlayerDeath()
    {
        OnPlayerDeath = null;
        OnPlayerRestart = null;
    }

    void Start()
    {
        m_direction = Vector3.zero;
        m_startingPos = new Vector3(1f, 3f, 0f);
    }

    public void ResumeBall(Vector2 _direction)
    {
        float xpos = Mathf.Clamp(transform.position.x, m_levelBorders.x, m_levelBorders.y);
        transform.position = new Vector3(xpos, transform.position.y, transform.position.z);
        gameObject.SetActive(true);
        m_speed = GameControl.control.m_LevelInfo.ballSpeed;
        StartCoroutine(InitializeBallCO(transform.position, _direction, true, 1f));
    }

    public void InitializeBall(Vector3 _position, Vector3 _direction, float _time, bool _randomize)
    {
        gameObject.SetActive(true);
        m_speed = GameControl.control.m_LevelInfo.ballSpeed;
        StartCoroutine(InitializeBallCO(_position, _direction, _randomize, _time));
    }

    public static void PlayerDeath()
    {
        if(OnPlayerDeath != null)
            OnPlayerDeath();
    }

    public static void PlayerRestart()
    {
        if (OnPlayerRestart != null)
            OnPlayerRestart();
    }

    IEnumerator InitializeBallCO(Vector3 _position, Vector3 _direction, bool randomize, float _time)
    {
        transform.position = _position;
    
        if (randomize)
        {
            transform.position  += Vector3.right * UnityEngine.Random.Range(-2f, 2f);
        }

        m_RB.velocity = _direction;
        m_powerUpManager.SwitchBoosters(false);

        m_gameManager.m_GameState = GAMESTATE.INIT;
        yield return new WaitForSeconds(_time - .3f);
        m_audio.clip = m_startClip;
        m_audio.Play();
        yield return new WaitForSeconds(.3f);
        m_gameManager.m_GameState = GAMESTATE.PLAY;

        m_powerUpManager.SwitchBoosters(true);

        m_velocity = _direction * m_speed;
        m_RB.velocity = m_velocity;
    }

    public void Stop()
    {
        if (!m_isStopped)
        {
            m_direction = m_RB.velocity.normalized;
            m_RB.velocity = Vector3.zero;
            m_isStopped = true;
        }   
    }

    public void Resume()
    {
        if (m_isStopped && m_direction != Vector3.zero)
        {
            m_RB.velocity = m_direction * m_speed;
            m_isStopped = false;
            m_direction = Vector3.zero;
        }     
    }

    void OnCollisionEnter2D(Collision2D _info) 
    {
        ContactPoint2D contactPoint = _info.contacts[0];
        if (_info.gameObject.CompareTag("Player"))
        {
            Ball.ContinousHits = 0;       
            ReflectFromPlayer();
            if(GameControl.control.m_LevelInfo.levelRestriction == LevelRestriction.MOVES)
            {
                m_scoreManager.TakeMove();
            }
        }

        else
        {
            if (_info.gameObject.CompareTag("Border"))
            {
                m_audio.clip = m_borderClip;
                m_audio.Play();
            }
            else if (_info.gameObject.CompareTag("Undestructable"))
            {
                m_audio.clip = m_undestructableClip;
                m_audio.Play();
            }
            Vector2 randomVector = new Vector2(UnityEngine.Random.Range(-.1f, .1f), UnityEngine.Random.Range(-.3f, .3f));
            if(Math.Abs(m_RB.velocity.y) <= 1f)
            {
                randomVector = new Vector2(UnityEngine.Random.Range(-.4f, .4f), UnityEngine.Random.Range(0f, -1));
            }
            m_RB.velocity += randomVector;
            m_RB.velocity = m_RB.velocity.normalized  * m_speed;
        }     
    }

    protected virtual void ReflectFromPlayer()
    {
        m_audio.clip = m_paddleClip;
        m_audio.Play();

        float distanceFromPlayerCenter = transform.position.x - m_player.position.x;

        m_velocity = m_RB.velocity.normalized;

        m_velocity = m_velocity.normalized;

        m_velocity += Vector2.right * distanceFromPlayerCenter / m_bounceOffser /** velocity.y*/;
                     
        m_RB.velocity = m_velocity.normalized * m_speed;   }

}
