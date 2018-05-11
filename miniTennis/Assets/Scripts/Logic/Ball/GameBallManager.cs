using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallManager
{
    private static GameBallManager m_instance;

    public static GameBallManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new GameBallManager();
            m_instance.Init();
        }

        return m_instance;
    }

    private List<Ball> m_gameBalls;

    private void Init()
    {
        m_gameBalls = new List<Ball>();
    }

    public Ball GetTargetBall()
    {
        return m_gameBalls[0];
    }

    public void RegisterBall(Ball ball)
    {
        if (m_gameBalls != null)
        {
            m_gameBalls.Add(ball);
        }
    }

    public void UnRegisterBall(Ball ball)
    {
        if (m_gameBalls != null)
        {
            m_gameBalls.Remove(ball);
        }
    }
}
