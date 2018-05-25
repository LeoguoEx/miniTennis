using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlBackBornPointState : AIControlState
{
    public AIControlBackBornPointState(Player player)
        : base(player)
    {
        m_statetype = EAIControlState.BackToBornPoint;
    }

    public override void EnterState()
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

    public override void UpdateState(GameBall ball)
    {
        Vector2 bornPosition = m_player.PlayerData.m_bornPosition;
        Vector2 curPosition = m_player.GetPlayerPosition();
        Vector2 moveDir = (bornPosition - curPosition).normalized;

        curPosition += Time.deltaTime * moveDir * m_player.PlayerData.m_moveSpeed;
        m_player.MovePosition(curPosition);

        float distance = Vector2.Distance(m_player.GetPlayerPosition(), bornPosition);
        if (distance <= 0.7f && SwitchStateAction != null)
        {
            SwitchStateAction(EAIControlState.Idle);
        }
    }

    public override void ExitState()
    {
    }

    private void HandlePlayerHitBallEvent(GameEvent eve)
    {
        if (eve == null) { return; }
        SwitchStateAction(EAIControlState.ChasingBall);
    }
}
