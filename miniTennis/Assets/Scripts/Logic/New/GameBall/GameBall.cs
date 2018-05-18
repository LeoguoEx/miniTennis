using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall
{
    private GameBallAnim m_ballAnim;
    private GameBallCollider m_ballCollider;
    private GameBallInstance m_ballInstance;
    
    public GameBall()
    {
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject ball = resModuel.LoadResources<GameObject>(EResourceType.Ball, "GameBall");
        ball = CommonFunc.Instantiate(ball);
        if (ball != null)
        {
            m_ballAnim = new GameBallAnim();
            m_ballCollider = ball.AddComponent<GameBallCollider>();
            m_ballInstance = ball.AddComponent<GameBallInstance>();
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.SetVelocity(velocity);
        }
    }

    public GameObject GetBallInstance()
    {
        if (m_ballInstance != null)
        {
            return m_ballInstance.gameObject;
        }

        return null;
    }
}
