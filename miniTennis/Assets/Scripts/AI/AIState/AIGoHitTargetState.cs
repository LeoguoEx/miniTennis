using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoHitTargetState : AIStateBase
{
	private Ball m_target;
	private float m_maxSpeed = 0.1f;
	
	public AIGoHitTargetState(EntityInstance entity) 
		: base(EAIStateType.GotoHitTarget, entity)
	{
	}

	public override void EnterState()
	{
		m_target = GameBallManager.GetInstance().GetTargetBall();
		if (m_target == null)
		{
			GameStart.GetInstance().LogModuel.Log(ELogType.Error, "TargetBall is null");
			return;
		}

		if (m_entity != null)
		{
			m_entity.Switch(EEntityState.Prepare, Vector2.zero);
		}
	}

	public override void UpdateState()
	{
		if (m_entity != null)
		{
			Vector3 position = m_entity.transform.position;
			Vector3 targetPosition = m_target.transform.position;
			position += m_maxSpeed * new Vector3(targetPosition.x - position.x, 0f, 0f);
			m_entity.SetMovePosition(position);
		}
	}

	public override void ExitState()
	{
	}
}
