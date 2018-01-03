using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPowerupElongate : BrickChange {
   
    [SerializeField]
    float m_scale;

    public override void HandleBallCollision(bool ballCollision, GameObject collidingBall = null)
    {    
        base.HandleBallCollision(ballCollision);
    }

    protected override void DestroyBrick(GameObject collidingBall = null)
    {
        if (m_gaveChange)
            return;
        m_gaveChange = true;
        m_playerPowerup.ChangeScale(m_time, m_scale);
        base.DestroyBrick();
    }
}
