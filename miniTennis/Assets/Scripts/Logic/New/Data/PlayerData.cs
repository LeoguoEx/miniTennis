using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string m_playerId;
    public string m_playerName;
    public string m_playerResPath;
    public string m_animControllerName;

    public Rect m_moveArea;
    public float m_radius;
    public float m_angle;

    public float m_moveSpeed;
    public Vector2 m_bornPosition;

    protected Vector2 m_firBallForceRange;
    protected float m_fireBallAngleRange;
    protected float m_playerMoveXRange;
    protected float m_playerForceY;

    public PlayerData()
    {
        m_playerId = "Player";
        m_playerName = "Player";
        m_playerResPath = "Player";
        m_animControllerName = "Character";

        m_moveArea = new Rect(-4.57f, -1.5f, 4.64f, -9.52f);
        m_radius = 2.5f;
        m_angle = 180;
        m_moveSpeed = 0.03f;
        m_firBallForceRange = new Vector2(10f, 16f);
        m_bornPosition = new Vector2(0f, -5.76f);
        m_fireBallAngleRange = 60f;
        m_playerMoveXRange = 1.5f;
        m_playerForceY = 2;
    }

    public virtual float GetFireBallForce(float y)
    {
        float value = y / m_playerForceY;
        return Mathf.Lerp(m_firBallForceRange.x, m_firBallForceRange.y, value);
    }

    public virtual float GetFireBallAngle(float x)
    {
        float value = x / m_playerMoveXRange;
        float angle = m_fireBallAngleRange * value;
        return angle;
    }
}
