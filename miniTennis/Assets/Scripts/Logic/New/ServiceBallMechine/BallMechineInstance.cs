using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMechineInstance : MonoBehaviour
{
    private Animator m_animator;
    
	void Start ()
	{
	    m_animator = GetComponent<Animator>();
	}

    public void PlayAnim(string name)
    {
        if (m_animator != null)
        {
            m_animator.Play(name);
        }
    }
}
