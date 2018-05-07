using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim
{
    private Animator m_animator;

    public CharacterAnim()
    {
        
    }

    public void InitAnimator(Animator animator)
    {
        m_animator = animator;
    }

    public void InitAnimController(RuntimeAnimatorController controller)
    {
        if (m_animator != null)
        {
            m_animator.runtimeAnimatorController = controller;
        }
    }

    public void PlayAnim(string animName)
    {
        if (m_animator != null)
        {
            m_animator.Play(animName);
        }
    }

}
