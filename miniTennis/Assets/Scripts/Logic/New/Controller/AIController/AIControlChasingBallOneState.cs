using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlChasingBallOneState : AIControlState
{

	public AIControlChasingBallOneState(Player player)
		:base(player)
	{
		m_statetype = EAIControlState.ChasingBall;
	}

	public override void EnterState()
	{
		m_player.StartMove();
	}

	public override void UpdateState(Transform ball)
	{
		if(ball == null) { return;}
		Vector3 position = ball.position;

		if (position.y < m_player.GetPlayerPosition().y)
		{
			Vector3 moveDir = position - m_player.GetPlayerPosition();
			moveDir = moveDir.normalized;

			position = m_player.GetPlayerPosition();
			position += Time.deltaTime * moveDir * m_player.PlayerData.m_moveSpeed;
			m_player.MovePosition(position);

			bool checkInArea = PlayerCollider.CheckInHitBallArea(ball, m_player.Transform,
				m_player.PlayerData.m_radius, m_player.PlayerData.m_angle, m_player.BoxCollider);
			if (checkInArea && SwitchStateAction != null)
			{
				SwitchStateAction(EAIControlState.Hit);
			}
		}
		else
		{
			if (SwitchStateAction != null)
			{
				SwitchStateAction(EAIControlState.BackToBornPoint);
			}
		}
	}

	public override void ExitState()
	{
	}
}
