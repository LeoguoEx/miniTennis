using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMechineInstance : MonoBehaviour
{
    private Animator m_animator;
    private GameObject m_fireBallPoint;

    private BallMechineFireBallEvent m_event;
    private float m_rate;
    private float m_rotationRate;

    public Action<Vector2, float> ServiceBall;

    private float m_time;
    private Vector3 m_leftPoint;
    private Vector3 m_rightPoint;
    
	void Start ()
	{
	    m_animator = GetComponent<Animator>();
	    m_fireBallPoint = CommonFunc.GetChild(gameObject, "FireBallPoint");
	}

    public void SetLeftRightPoint(Vector3 leftPoint, Vector3 rightPoint)
    {
        m_leftPoint = leftPoint;
        m_rightPoint = rightPoint;
    }

    private void Update()
    {
        if (m_event != null)
        {
            Vector3 position = gameObject.transform.position;
            
            position += Time.deltaTime * new Vector3(m_event.m_horizontalSpeed, 0f, 0f) * m_rate;
            if (position.x > 3.5f)
            {
                m_rate = -1;
            }
            else if (position.x < -3.5f)
            {
                m_rate = 1;
            }
            gameObject.transform.position = position;
            
            Vector3 leftDir = (m_leftPoint - position).normalized;
            Vector3 rightDir = (m_rightPoint - position).normalized;
            Vector3 rotation = gameObject.transform.rotation.eulerAngles;
            rotation += Time.deltaTime * new Vector3(0f, 0f, m_event.m_rotateSpeed) * m_rotationRate;
            Vector3 dir = transform.rotation * Vector3.up;
            float leftDot = Vector3.Dot(dir, leftDir);
            float rightDot = Vector3.Dot(rightDir, dir);
            if (leftDot>0.95 && leftDot <1.05)
            {
                m_rotationRate = 1;
            }
            else if (rightDot> 0.95 && rightDot < 1.05)
            {
                m_rotationRate = -1;
            }
            
            gameObject.transform.rotation = Quaternion.Euler(rotation);

            if (Time.time >= m_time + m_event.m_fireTime)
            {
                if (ServiceBall != null)
                {
                    ServiceBall(dir, m_event.m_force);
                }

                m_time = Time.time + 1f;
            }
        }
    }

    public void PlayAnim(string name)
    {
        if (m_animator != null)
        {
            m_animator.enabled = true;
            m_animator.Play(name);
        }
    }

    public Vector3 GetFireBallWorldPoint()
    {
        if (m_fireBallPoint != null)
        {
            return m_fireBallPoint.transform.position;
        }
        return Vector3.zero;
    }

    public void SetEvent(BallMechineFireBallEvent eve)
    {
        if(eve == null){return;}

        m_event = eve;
        m_rate = 1;
        m_rotationRate = -1;
        m_time = Time.time;
    }
}
