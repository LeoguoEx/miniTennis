﻿using UnityEngine;

public enum ESide
{
    Player = 0,
    AI = 1,
    None = 2,
}

public class GroundData
{
    private Vector3 m_userFireBallPoint;
    private Vector3 m_aiFireBallPoint;

    public GroundData()
    {
        m_userFireBallPoint = new Vector3(0f, -3.54f, 0f);
        m_aiFireBallPoint = new Vector3(0f, 3.54f, 0f);
    }

    public Vector3 GetFireBallPoint(ESide seriveSide)
    {
        switch (seriveSide)
        {
            case ESide.Player:
                return m_userFireBallPoint;
            case ESide.AI:
                return m_aiFireBallPoint;
        }
        return Vector3.zero;
    }

}
