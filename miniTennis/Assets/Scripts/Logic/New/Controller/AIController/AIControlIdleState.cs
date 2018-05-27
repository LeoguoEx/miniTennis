using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlIdleState : AIControlState
{
    public AIControlIdleState(Player player) : base(player)
    {
        m_statetype = EAIControlState.Idle;
    }

    public override void EnterState()
    {
        m_player.SetIdle();
    }

    public override void UpdateState(Transform ball)
    {

    }

    public override void ExitState()
    {

    }

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.RegisterEventListener(GameEventID.PLAYER_HIT_BALL, HandlePlayerHitBallEvent);
    }

    public override void UnRegisterEvent()
    {
        base.UnRegisterEvent();

        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.UnRegisterEventListener(GameEventID.PLAYER_HIT_BALL, HandlePlayerHitBallEvent);
    }

    private void HandlePlayerHitBallEvent(GameEvent eve)
    {
        if(eve == null) { return; }
        
        SwitchStateAction(EAIControlState.ChasingBall);
    }
}
