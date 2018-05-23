using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall
{
    private GameBallAnim m_ballAnim;
    private GameBallInstance m_ballInstance;
    private Action<GameBall> m_outOfRangeAction;
    
    public GameBall(BallData data)
    {
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject ball = resModuel.LoadResources<GameObject>(EResourceType.Ball, "GameBall");
        ball = CommonFunc.Instantiate(ball);
        if (ball != null)
        {
            m_ballAnim = new GameBallAnim();
            m_ballInstance = ball.AddComponent<GameBallInstance>();
            m_ballInstance.SetBallRect(data.m_ballBoundArea);
        }
    }

    public void Destory()
    {
        GameObject.Destroy(m_ballInstance.gameObject);
        m_ballAnim = null;
        m_outOfRangeAction = null;
    }

    public void SetActive(bool active)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.gameObject.SetActive(active);
        }
    }

    public void SetOutofRangeAction(Action<GameBall> outOfRangeAction)
    {
        m_outOfRangeAction = outOfRangeAction;
        if (m_ballInstance != null)
        {
            m_ballInstance.BallOutofRangeAction = BallOutofRange;
        }
    }

    public void SetVelocity(Vector2 dir, float force)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.SetVelocity(dir, force);
        }
    }

    public void ResetVelocity()
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.FresetVelocity();
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

    public Vector3 GetPosition()
    {
        if (m_ballInstance != null)
        {
            return m_ballInstance.transform.position;
        }
        return Vector3.zero;
    }

    public void SetPosition(Vector2 pos)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.SetPosition(pos);
        }
    }

    private void BallOutofRange()
    {
        if (m_outOfRangeAction != null)
        {
            m_outOfRangeAction(this);
        }
    }
}
