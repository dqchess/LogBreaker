using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPowerupChangeDirection : BrickChange {

   
    public override void HandleBallCollision(bool ballCollision, GameObject collidingBall = null)
    {
        base.HandleBallCollision(ballCollision);
    }

    protected override void DestroyBrick(GameObject collidingBall = null)
    {
        if (m_gaveChange)
            return;

        m_gaveChange = true;
        m_playerPowerup.ChangeDirection(m_time);
        base.DestroyBrick();
    }
}
