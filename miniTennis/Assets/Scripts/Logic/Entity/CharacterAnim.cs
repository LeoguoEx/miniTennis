using UnityEngine;

[RequireComponent(typeof(EntityInstance))]
public class CharacterAnim
{
    private Animator m_animator;

    public CharacterAnim(GameObject go)
    {
        m_animator = CommonFunc.AddSingleComponent<Animator>(go);
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

    public void PlayAnim(EEntityState animType)
    {
        if (m_animator != null)
        {
            string animName = GetAnimNameByType(animType);
            m_animator.Play(animName);
        }
    }

    private string GetAnimNameByType(EEntityState animType)
    {
        return animType.ToString();
    }
}
