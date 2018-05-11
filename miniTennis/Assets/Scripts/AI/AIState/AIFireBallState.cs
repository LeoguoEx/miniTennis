using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFireBallState : AIStateBase
{
	private Vector2 m_fireRange = new Vector2(-0.3f, 0.3f);
	public AIFireBallState(EntityInstance entity)
		: base(EAIStateType.FireBall, entity)
	{
	}

	public override void EnterState()
	{
		if (m_entity != null)
		{
			Vector2 dir = m_entity.GetForwardDir();
			Vector2 position =  new Vector2(Random.Range(m_fireRange.x, m_fireRange.y), dir.y);
			m_entity.Switch(EEntityState.Hit, position.normalized);

			GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
			eventModuel.SendEvent(GameEventID.AI_SWITCH_STATE, false, 0.3f, EAIStateType.GotoHitTarget);
		}
	}

	public override void UpdateState()
	{
	}

	public override void ExitState()
	{
	}
}
