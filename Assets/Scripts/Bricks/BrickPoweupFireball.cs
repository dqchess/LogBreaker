using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPoweupFireball : Brick {

    [Header("References")]

    [SerializeField]
    GameObject _explosionEffectPrefab;

    [SerializeField]
    Ball m_ballPrefab;

    Vector2 m_ballCollisionPos;

    protected override void Awake()
    {
        base.Awake();
        m_ballCollisionPos = Vector2.zero;          
    }
 
    public override void HandleBallCollision(bool _ballCollision, GameObject _collidingBall = null)
    {       
        if(_collidingBall != null)
        {           
            m_ballCollisionPos = _collidingBall.transform.position;
            
        }            
        base.HandleBallCollision(_ballCollision);                 
    }

    protected override void DestroyBrick(GameObject _collidingBall = null)
    {
        if (m_wasDestroyed)
            return;
        m_wasDestroyed = true;
        Instantiate(_explosionEffectPrefab, transform.position, transform.rotation);
        
        base.DestroyBrick();
        Vector2 direction = Vector2.left;
        if(m_ballCollisionPos != Vector2.zero)
        {
            if(m_ballCollisionPos.x > transform.position.x + .85f)
            {
                direction = Vector2.left;
            }
            else if (m_ballCollisionPos.x < transform.position.x - .85f)
            {
                direction = Vector2.right;
            }
            else
            {
                if(Mathf.Abs( m_ballCollisionPos.x - transform.position.x) <= .85f)
                {
                    if(m_ballCollisionPos.y > transform.position.y)
                    {
                        direction = Vector2.down;
                    }
                    else if(m_ballCollisionPos.y < transform.position.y)
                    {
                        direction = Vector2.up;
                    }
                    
                }
            }
        }
        m_gameManager.MakeBall(0f, transform.position , direction * 1f);
    }
}
