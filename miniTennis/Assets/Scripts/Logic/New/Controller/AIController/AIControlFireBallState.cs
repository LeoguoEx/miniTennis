using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlFireBallState : AIControlState
{
    public AIControlFireBallState(Player player) : base(player)
    {
        m_statetype = EAIControlState.FireBall;
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState(GameBall ball)
    {
    }

    public override void ExitState()
    {
    }
}
