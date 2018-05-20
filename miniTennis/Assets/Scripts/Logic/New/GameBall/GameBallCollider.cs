using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallCollider : MonoBehaviour
{
    public Action<Collision2D> m_colliderAction;
    
	void Start () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (m_colliderAction != null)
        {
            m_colliderAction(other);
        }
    }
}
