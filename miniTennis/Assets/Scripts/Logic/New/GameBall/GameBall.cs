using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall
{
    private GameBallAnim m_ballAnim;
    private GameBallInstance m_ballInstance;
    
    public GameBall()
    {
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject ball = resModuel.LoadResources<GameObject>(EResourceType.Ball, "GameBall");
        ball = CommonFunc.Instantiate(ball);
        if (ball != null)
        {
            m_ballAnim = new GameBallAnim();
            m_ballInstance = ball.AddComponent<GameBallInstance>();
        }
    }

    public void SetVelocity(Vector2 dir, float force)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.SetVelocity(dir, force);
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

    public void SetPosition(Vector2 pos)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.SetPosition(pos);
        }
    }
}
