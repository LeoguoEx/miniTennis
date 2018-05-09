using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateController : MonoBehaviour
{
    private Dictionary<int, AIStateBase> m_aiStateDic;
    private AIStateBase m_curStateBase;
    private EAIStateType m_curAIStateType;

    private void Start()
    {
        m_aiStateDic = new Dictionary<int, AIStateBase>();
        m_aiStateDic.Add((int)EAIStateType.Idle, new AIIdleState());
        m_aiStateDic.Add((int)EAIStateType.FireBall, new AIFireBallState());
        m_aiStateDic.Add((int)EAIStateType.GotoHitTarget, new AIGoHitTargetState());
        m_aiStateDic.Add((int)EAIStateType.Hit, new AIHitState());
        m_aiStateDic.Add((int)EAIStateType.Prepare, new AIPrepareState());

        m_curAIStateType = EAIStateType.None;
        SwitchState(EAIStateType.Idle);
    }

    public void SwitchState(EAIStateType stateType)
    {
        if (m_curAIStateType == stateType)
        {
            return;
        }
        if (m_curStateBase != null)
        {
            m_curStateBase.PreExitState();
            m_curStateBase.ExitState();
        }

        m_aiStateDic.TryGetValue((int) m_curAIStateType, out m_curStateBase);
        if (m_curStateBase != null)
        {
            m_curStateBase.PreEnterState();
            m_curStateBase.EnterState();
            m_curAIStateType = stateType;
        }
    }

    private void Update()
    {
        if (m_curStateBase != null)
        {
            m_curStateBase.UpdateState();
        }
    }
}
