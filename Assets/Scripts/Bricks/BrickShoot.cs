using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickShoot : Brick{

    [Header("References")]

    [SerializeField]
    GameObject m_explosionEffectPrefab;

    [SerializeField]
    FlowerFall m_flowerEffectPrefab;

    [Header("Variables")]

    [SerializeField]
    float m_shootDuration;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void HandleBallCollision(bool _ballCollision, GameObject _collidingBall = null)
    {
        base.HandleBallCollision(_ballCollision);
    }

    protected override void DestroyBrick(GameObject collidingBall = null)
    {       
        Destroy();
        base.DestroyBrick();            
    }

    void Destroy()
    {
        FlowerFall flowerFall =  Instantiate(m_flowerEffectPrefab, transform.position, Quaternion.identity);
        flowerFall.Initialize(m_shootDuration);
        gameObject.SetActive(false);
    }
}
