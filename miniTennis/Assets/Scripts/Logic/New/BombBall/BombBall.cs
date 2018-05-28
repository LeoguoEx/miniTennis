using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall
{
    private BombInstance m_ballInstance;
    private Action<BombBall> m_outOfRangeAction;

    private bool m_start;
    private float m_endTime;

    private ESide m_side;
    private bool m_needOffset;
    private Vector2 m_offsetDir;

    public BombBall(BallData data, Action<string> action = null)
    {
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject ball = resModuel.LoadResources<GameObject>(EResourceType.Ball, "BombBall");
        ball = CommonFunc.Instantiate(ball);
        if (ball != null)
        {
            m_ballInstance = ball.AddComponent<BombInstance>();
            m_ballInstance.SetBallRect(data.m_ballBoundArea);
            m_ballInstance.SetBounceAction(action);
        }
    }


    public void Destory()
    {
        GameObject.Destroy(m_ballInstance.gameObject);
        m_outOfRangeAction = null;
    }

    public void SetActive(bool active)
    {
        if (m_ballInstance != null)
        {
            m_ballInstance.gameObject.SetActive(active);
        }
    }

    public void SetOutofRangeAction(Action<BombBall> outOfRangeAction)
    {
        m_outOfRangeAction = outOfRangeAction;
        if (m_ballInstance != null)
        {
            m_ballInstance.BallOutofRangeAction = BallOutofRange;
        }
    }

    public void SetVelocity(Vector2 dir, float force, ESide side)
    {
        if (m_ballInstance != null)
        {
            m_needOffset = side == m_side;
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
            m_ballInstance.PlayBounceUpAndDown();
        }
    }

    private void BallOutofRange()
    {
        if (m_outOfRangeAction != null)
        {
            m_outOfRangeAction(this);
        }
    }

    public void Update()
    {
    }

    public void ChangeSide(ESide side)
    {
        m_side = side;
    }

    public void PlayBomb()
    {
        m_ballInstance.SetBoom();
    }

    public void SetBombScale(float value)
    {
        m_ballInstance.SetScale(value);
    }
}
