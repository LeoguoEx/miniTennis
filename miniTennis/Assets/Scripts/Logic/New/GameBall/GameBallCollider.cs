using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallCollider : MonoBehaviour
{
    public Action<Collision2D> m_colliderAction;

    private bool m_bounce;
    
	void Start ()
	{
	    m_bounce = false;
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (m_colliderAction != null)
        {
            m_bounce = true;
            m_colliderAction(other);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!m_bounce)
        {
            m_bounce = true;
            if (m_colliderAction != null)
            {
                m_colliderAction(other);
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        m_bounce = false;
    }
}
