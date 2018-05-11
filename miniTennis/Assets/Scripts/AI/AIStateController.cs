using System.Collections.Generic;
using UnityEngine;

public class AIStateController : MonoBehaviour
{
    private Dictionary<int, AIStateBase> m_aiStateDic;
    private AIStateBase m_curStateBase;
    private EAIStateType m_curAIStateType;

    private void Start()
    {
        EntityInstance instance = gameObject.GetComponent<EntityInstance>();
        m_aiStateDic = new Dictionary<int, AIStateBase>();
        m_aiStateDic.Add((int)EAIStateType.Idle, new AIIdleState(instance));
        m_aiStateDic.Add((int)EAIStateType.FireBall, new AIFireBallState(instance));
        m_aiStateDic.Add((int)EAIStateType.GotoHitTarget, new AIGoHitTargetState(instance));
        m_aiStateDic.Add((int)EAIStateType.Hit, new AIHitState(instance));
        m_aiStateDic.Add((int)EAIStateType.Prepare, new AIPrepareState(instance));

        m_curAIStateType = EAIStateType.None;
        SwitchState(EAIStateType.Idle);

        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.RegisterEventListener(GameEventID.AI_SWITCH_STATE, HandleSwitchState);
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

        m_aiStateDic.TryGetValue((int) stateType, out m_curStateBase);
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

    private void HandleSwitchState(GameEvent eve)
    {
        EAIStateType state = eve.GetParamByIndex<EAIStateType>(0);
        SwitchState(state);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            SwitchState(EAIStateType.FireBall);
        }
    }
}
