using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlHitState : AIControlState
{
    public AIControlHitState(Player player) : base(player)
    {
        m_statetype = EAIControlState.Hit;
    }

    public override void EnterState()
    {
        m_player.EndMove();
        CoroutineTool.GetInstance().StartGameCoroutine(WaitForHitAnimEnd());
    }

    public override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    public override void UnRegisterEvent()
    {
        base.UnRegisterEvent();
    }

    public override void UpdateState(Transform ball)
    {
    }

    public override void ExitState()
    {
    }

    private IEnumerator WaitForHitAnimEnd()
    {
        yield return new WaitForSeconds(0.4f);
        SwitchStateAction(EAIControlState.BackToBornPoint);
    }
}
