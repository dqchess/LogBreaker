using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBomb : Brick {

    [Header("References")]

    [SerializeField]
    GameObject m_explosionPrefab;

    ParticleSystem m_flaseEffect;

    AudioSource m_audio;

    [Header("Variables")]

    [SerializeField]
    int m_range;

    protected override void Awake()
    {
        base.Awake();
        m_audio = GetComponent<AudioSource>();
        m_flaseEffect = transform.Find("Flare").GetComponent<ParticleSystem>();      
    }

    void OnEnable()
    {
        if(HitPoints <= 1)
        {
            m_flaseEffect.Play();
            m_audio.Play();
        }
    }

    protected override void DestroyBrick(GameObject _collidingBall = null)
    {
        if (m_wasDestroyed)
            return;
        m_wasDestroyed = true;
        Explode();
        base.DestroyBrick();
    }

    public void Explode()
    {
        Collider2D[] bricks = Physics2D.OverlapBoxAll(transform.position, new Vector2(4f, 1f) * m_range, 0f);

        for (int i = 0; i < bricks.Length; i++)
        {

            if (bricks[i].transform.tag.Split(' ')[0] == "Brick" &&
                bricks[i].transform.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
            {

                Brick brickScript = bricks[i].transform.GetComponent<Brick>();
                if (brickScript)
                {
                    if (brickScript.HitPoints > 0)
                    {
                        brickScript.HandleBallCollision(false);
                    }
                }
            }
        }

        Instantiate(m_explosionPrefab, transform.position, transform.rotation);
    }

    public override void HandleBallCollision(bool _isBallCollision, GameObject _collidingBall = null)
    {      
        base.HandleBallCollision(_isBallCollision);
        if(HitPoints <= 1)
        {
            m_flaseEffect.Play();
            m_audio.Play();
        }
       
    }

    protected override void OnParticleCollision(GameObject _other)
    {
        base.OnParticleCollision(_other);
        if(HitPoints <= 1)
        {
            if (!m_flaseEffect.isPlaying)
            {
                m_flaseEffect.Play();
                m_audio.Play();
            }           
        }
    }
}
