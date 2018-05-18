using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim
{
	private const string CHARACTER = "Character";
	
	private Animator m_animator;

	public PlayerAnim(GameObject gameObject)
	{
		if(gameObject == null){return;}

		GameObject player = CommonFunc.GetChild(gameObject, CHARACTER);
		m_animator = CommonFunc.AddSingleComponent<Animator>(player);
	}

	public void InitAnimator(string controllerName)
	{
		if(m_animator == null){return;}
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		RuntimeAnimatorController controller = resModuel.LoadResources<RuntimeAnimatorController>(EResourceType.AnimController, controllerName);
		m_animator.runtimeAnimatorController = controller;
	}

	public void PlayAnim(EEntityState animType)
	{
		string animName = GetAnimNameByType(animType);
		PlayAnim(animName);
	}

	public void PlayAnim(string animName)
	{
		if (m_animator != null)
		{
			m_animator.Play(animName);
		}
	}

	private string GetAnimNameByType(EEntityState animType)
	{
		return animType.ToString();
	}
	
}
