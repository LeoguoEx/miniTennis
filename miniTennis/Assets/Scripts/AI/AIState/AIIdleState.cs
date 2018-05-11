using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIStateBase
{

	public AIIdleState(EntityInstance entity) 
		: base(EAIStateType.Idle, entity)
	{
	}

	public override void EnterState()
	{
		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.RegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleBallHitEvent);

		if (m_entity != null)
		{
			m_entity.Switch(EEntityState.Idle, Vector3.zero);
		}
	}

	public override void UpdateState()
	{
	}

	public override void ExitState()
	{
		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.UnRegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleBallHitEvent);
	}

	private void HandleBallHitEvent(GameEvent eve)
	{
		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.SendEvent(GameEventID.AI_SWITCH_STATE, true, 0f, EAIStateType.GotoHitTarget);
	}
}
