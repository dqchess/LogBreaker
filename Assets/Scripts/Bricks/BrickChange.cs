using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickChange : Brick {

    [SerializeField]
    protected float m_time;
    
    protected bool m_gaveChange;

    Animator m_anim;

    AudioSource m_audio;  

    protected override void Awake()
    {
        m_gaveChange = false;
        GetReferences();
        base.Awake();
    }

    void GetReferences()
    {
        m_audio = GetComponent<AudioSource>();
        m_anim = GetComponent<Animator>();
    }

    public override void HandleBallCollision(bool _ballCollision, GameObject _collidingBall = null)
    {
        if (m_gaveChange)
            return;
        base.HandleBallCollision(_ballCollision);
    }

    protected override void DestroyBrick(GameObject _collidingBall = null)
    {
        m_audio.Play();
        m_anim.SetTrigger("Destroy_t");       
    }

    public void Destroy()
    {
        base.DestroyBrick();
    }
}
