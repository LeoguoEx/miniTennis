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

    private Vector2 m_firBallForceRange;
    
    public PlayerData()
    {
        
    }

    public float GetFireBallForce(Vector2 direction, Vector2 playerToTargetDirection)
    {
        float value = Vector2.Dot(direction, playerToTargetDirection);
        value = Mathf.Abs(value);
        value = Mathf.Abs(1 - value);
        return Mathf.Lerp(m_firBallForceRange.x, m_firBallForceRange.y, value);
    }
}
