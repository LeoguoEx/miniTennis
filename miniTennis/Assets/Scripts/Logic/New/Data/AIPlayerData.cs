using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerData : PlayerData
{
    public AIPlayerData()
    {
        m_playerId = "AI";
        m_playerName = "AI";
        m_playerResPath = "Player";
        m_animControllerName = "Character";

        m_moveArea = new Rect(-4.57f, 8.15f, 4.64f, 4f);
        m_radius = 2.5f;
        m_angle = 180;
        m_moveSpeed = 8f;
        m_firBallForceRange = new Vector2(7f, 15f);
        m_bornPosition = new Vector2(0f, 7.8f);
        m_fireBallAngleRange = 60f;
        m_playerMoveXRange = 1.5f;
        m_playerForceY = 2;
    }
    
    public override float GetFireBallForce(float y)
    {
        float value = Random.Range(m_firBallForceRange.x, m_firBallForceRange.y);
        return value;
    }

    public override float GetFireBallAngle(float x)
    {
        float angle = Random.RandomRange(-m_fireBallAngleRange, m_fireBallAngleRange);
        return angle;
    }
}
