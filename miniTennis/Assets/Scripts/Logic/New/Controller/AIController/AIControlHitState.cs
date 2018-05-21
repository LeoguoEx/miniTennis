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
        SwitchStateAction(EAIControlState.BackToBornPoint);
    }

    public override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    public override void UnRegisterEvent()
    {
        base.UnRegisterEvent();
    }

    public override void UpdateState(GameBall ball)
    {

    }

    public override void ExitState()
    {
    }
}
