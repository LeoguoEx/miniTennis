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
        m_radius = 3f;
        m_angle = 120;
        m_moveSpeed = 10f;
        m_firBallForceRange = new Vector2(7f, 15f);
        m_bornPosition = new Vector2(0f, 7.8f);
        m_fireBallAngleRange = 60f;
        m_playerMoveXRange = 1.5f;
        m_playerForceY = 2;
    }
}
