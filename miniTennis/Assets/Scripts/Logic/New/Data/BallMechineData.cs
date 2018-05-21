using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMechineFireBallEvent
{
    public float m_force;
    public float m_horizontalSpeed;
    public float m_rotateSpeed;
    public float m_fireTime;
    private Vector2 m_fireBallTime;
    private float m_duringtime;

    public BallMechineFireBallEvent(float force, float horizontal, float rotateSpeed, Vector2 fireBallTime)
    {
        m_force = force;
        m_horizontalSpeed = horizontal;
        m_rotateSpeed = rotateSpeed;
        m_fireBallTime = fireBallTime;
        m_fireTime = GetRandomFireBallTime();
        m_duringtime = 5f;
    }

    public float GetRandomFireBallTime()
    {
        return Random.Range(m_fireBallTime.x, m_fireBallTime.y);
    }
}

public class BallMechineData
{
    public Vector3 m_mechinePosition;
    public Vector3 m_mechineRotation;
    public int m_fireBallCountOneRound;

    public string m_servieBallAnim;
    private List<BallMechineFireBallEvent> m_eventList;
    private Queue<BallMechineFireBallEvent> m_eventQueue;

    public BallMechineData()
    {
        m_mechinePosition = new Vector3(0f, 8.34f, 0f);
        m_mechineRotation = new Vector3(0f, 0f, 180f);
        m_fireBallCountOneRound = 3;
        m_servieBallAnim = "FireBallMechine";
        
        m_eventList = new List<BallMechineFireBallEvent>();
        m_eventQueue = new Queue<BallMechineFireBallEvent>();
        m_eventList.Add(new BallMechineFireBallEvent(10f, 0f, 0f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(20f, 0f, 0f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(10f, 1f, 0f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(10f, 2f, 0f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(10f, 0f, 10f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(10f, 0f, 15f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(10f, 1f, 15f, new Vector2(0f, 2f)));
        m_eventList.Add(new BallMechineFireBallEvent(20f, 2f, 15f, new Vector2(0f, 2f)));
    }

    public void InitEvents()
    {
        for (int i = 0; i < m_eventList.Count; i++)
        {
            m_eventQueue.Enqueue(m_eventList[i]);
        }
    }

    public BallMechineFireBallEvent PopEvent()
    {
        if (m_eventQueue.Count == 0)
        {
            InitEvents();
        }

        return m_eventQueue.Dequeue();
    }
}
