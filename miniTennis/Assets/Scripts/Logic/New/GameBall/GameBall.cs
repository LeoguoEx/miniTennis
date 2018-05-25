using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall
{
    private GameBallAnim m_ballAnim;
    private GameBallInstance m_ballInstance;
    private Action<GameBall> m_outOfRangeAction;

    private bool m_start;
    private float m_endTime;

    private ESide m_side;
    private bool m_needOffset;
    private Vector2 m_offsetDir;
    
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
        if (m_start)
        {
            if (Time.time > m_endTime)
            {
                m_start = false;

                if (m_ballInstance != null)
                {
                    m_ballInstance.SetOffsetDir(Vector2.zero, false);
                }
                
                GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
                eveModuel.SendEvent(GameEventID.END_GAME_EVENT, true, 0f);
            }
            else
            {
                m_ballInstance.SetOffsetDir(m_offsetDir, m_needOffset);
            }
        }
    }
    
    public void ExcuteEffect(EffectBase effect, ESide side)
    {
        if (effect != null && effect is EffectBananaBall)
        {
            EffectBananaBall effectBananaBall = effect as EffectBananaBall;
            m_start = true;
            m_endTime = effectBananaBall.m_duringTime + Time.time;
            m_side = side;
            ChangeEffectDir(m_side);
        }
    }

    public void ChangeEffectDir(ESide side)
    {
        if (m_start && m_side == side)
        {
            float value = UnityEngine.Random.Range(0f, 1f);
            m_offsetDir = (value >= 0.5f) ? new Vector2(0.02f, 0f) : new Vector2(-0.02f, 0f);
        }
    }
}
