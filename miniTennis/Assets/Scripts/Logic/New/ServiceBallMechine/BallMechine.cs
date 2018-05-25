using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMechine
{
    private BallMechineInstance m_instance;
    private BallMechineData m_mechineData;
    private BallData m_ballData;

    private List<GameBall> m_ballList;
    private List<GameBall> m_cacheBallList;

    private int m_fireBallIndex;

    public BallMechine(BallMechineData data, BallData ballData, Vector3 leftPoint, Vector3 rightPoint)
    {
        m_mechineData = data;
        m_ballData = ballData;
        m_ballList = new List<GameBall>();
        m_cacheBallList = new List<GameBall>();

        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject mechine = resModuel.LoadResources<GameObject>(EResourceType.BallMechine, "BallMechine");
        mechine = CommonFunc.Instantiate(mechine);
        m_instance = mechine.AddComponent<BallMechineInstance>();
        mechine.transform.position = data.m_mechinePosition;
        mechine.transform.rotation = Quaternion.Euler(data.m_mechineRotation);
        m_instance.ServiceBall = ServieBall;
        m_instance.SetLeftRightPoint(leftPoint, rightPoint);
    }

    public void Destory()
    {
        GameObject.Destroy(m_instance);
        m_instance = null;
        m_mechineData = null;
        m_ballData = null;
        for (int i = 0; i < m_ballList.Count; i++)
        {
            m_ballList[i].Destory();
        }

        for (int i = 0; i < m_cacheBallList.Count; i++)
        {
            m_cacheBallList[i].Destory();
        }
    }

    public void ServieBall(Vector2 dir, float force)
    {
        if (m_instance != null)
        {
            m_instance.enabled = true;
            m_instance.PlayAnim(m_mechineData.m_servieBallAnim);
        }

        GameBall ball = CreateGameBall();
        CoroutineTool.GetInstance().StartGameCoroutine(FireBall(ball, dir, force));
    }

    private IEnumerator FireBall(GameBall ball, Vector2 dir, float force)
    {
        yield return new WaitForEndOfFrame();
        ball.SetOutofRangeAction(HandleBallOutOfRangeAction);
        ball.SetActive(true);
        ball.SetPosition(m_instance.GetFireBallWorldPoint());
        ball.SetVelocity(dir, force, ESide.None);
    }

    private GameBall CreateGameBall()
    {
        GameBall ball = null;
        if (m_cacheBallList.Count > 0)
        {
            ball = m_cacheBallList[0];
            m_cacheBallList.RemoveAt(0);
        }
        else
        {
            ball = new GameBall(m_ballData);
        }
        m_ballList.Add(ball);
        return ball;
    }

    private List<GameBall> m_checkBalls;
    public GameBall[] GetInPlayerAreaBalls(Player player)
    {
        if(m_checkBalls == null) { m_checkBalls = new List<GameBall>();}
        m_checkBalls.Clear();
        for (int i = 0; i < m_ballList.Count; i++)
        {
            GameBall ball = m_ballList[i];
            bool inarea = PlayerCollider.CheckInHitBallArea(ball.GetBallInstance().transform, player.Transform,
                player.PlayerData.m_radius, player.PlayerData.m_angle, player.BoxCollider);
            if (inarea)
            {
                m_checkBalls.Add(ball);
            }
        }
        return m_checkBalls.ToArray();
    }

    private void HandleBallOutOfRangeAction(GameBall ball)
    {
        if(ball == null){return;}
        ball.ResetVelocity();
        ball.SetActive(false);
        m_ballList.Remove(ball);
        m_cacheBallList.Add(ball);
        m_fireBallIndex++;
        if (m_mechineData.m_fireBallCountOneRound < m_fireBallIndex)
        {
            m_instance.ServiceBall = null;
            CoroutineTool.GetInstance().StartCoroutine(StartFireBall());
            m_fireBallIndex = 0;
        }
    }

    private IEnumerator StartFireBall()
    {
        yield return new WaitForSeconds(3f);
        m_instance.ServiceBall = ServieBall;
        StartEvent();
    }

    public void StartEvent()
    {
        BallMechineFireBallEvent eve = m_mechineData.PopEvent();
        HandleMechineEvent(eve);
    }

    public void HandleMechineEvent(BallMechineFireBallEvent eve)
    {
        if (m_instance != null)
        {
            m_instance.SetEvent(eve);
        }
    }
}
