using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : PlayerControllerBase
{
    private Dictionary<int, AIControlState> m_stateDic;
    private AIControlState m_curState;

    private GameBall m_target;

	public override void InitController(Player player)
	{
	    AIControlState.SwitchStateAction = SwitchState;

        m_stateDic = new Dictionary<int, AIControlState>();
        m_stateDic.Add((int)EAIControlState.Idle, new AIControlIdleState(player));
        m_stateDic.Add((int)EAIControlState.BackToBornPoint, new AIControlBackBornPointState(player));
        m_stateDic.Add((int)EAIControlState.ChasingBall, new AIControlChasingBallState(player));
        m_stateDic.Add((int)EAIControlState.FireBall, new AIControlFireBallState(player));
        m_stateDic.Add((int)EAIControlState.Hit, new AIControlHitState(player));

        SwitchState(EAIControlState.Idle);
	}

    public void SetGameBall(GameBall ball)
    {
        m_target = ball;
    }

    private void SwitchState(EAIControlState state)
    {
        if (m_curState != null)
        {
            if (m_curState.StateType == state)
            {
                return;
            }
            m_curState.PreExitState();
            m_curState.UnRegisterEvent();
            m_curState.ExitState();
        }

        m_stateDic.TryGetValue((int) state, out m_curState);
        if (m_curState != null)
        {
            m_curState.PreEnterState();
            m_curState.RegisterEvent();
            m_curState.EnterState();
        }
    }

    void Update()
    {
        if (m_curState != null)
        {
            m_curState.UpdateState(m_target);
        }
    }

	public override void DestroyController()
	{
		GameObject.Destroy(this);
	}
}
