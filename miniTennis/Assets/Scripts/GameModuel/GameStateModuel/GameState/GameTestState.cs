﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestState : GameStateBase
{
    private Ball m_ball;
    private EntityInstance m_entityInstance;
    private EntityInstance m_aiInstance;
    private GameTestStateUI m_ui;
    
    public GameTestState(EGameStateType stateType) 
        : base(stateType)
    {
        StateType = stateType;
    }

    public override void EnterState()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.RegisterEventListener(GameEventID.Reset_Game_State, HandleResetStateEvent);
        eventModuel.RegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleEntityHitBall);
        
        GameObject ball = GameObject.Find("Ball");
        if (ball != null)
        {
            m_ball = CommonFunc.AddSingleComponent<Ball>(ball);
            m_ball.Reset();
        }
        
        GameObject chracter  =GameObject.Find("Character");
        if (chracter != null)
        {
            m_entityInstance = chracter.GetComponent<EntityInstance>();
	
            Vector4 moveRange = new Vector4(-1.73f, -0.81f, 3.15f, -5.4f);
            m_entityInstance.SetMoveRange(moveRange);
        }


        GameObject ai = GameObject.Find("AI");
        if (ai != null)
        {
            m_aiInstance = ai.GetComponent<EntityInstance>();
            Vector4 moveRange = new Vector4(-2.09f, 9.3f, 2.61f, 4.61f);
            m_aiInstance.SetMoveRange(moveRange);
            ai.AddComponent<AIStateController>();
        }

        GameDataModuel dataModuel = GameStart.GetInstance().DataModuel;
        dataModuel.SetGameData(3, 0);

        m_ui = GameObject.FindObjectOfType<GameTestStateUI>();
        if (m_ui != null)
        {
            m_ui.Init();
        }
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.UnRegisterEventListener(GameEventID.Reset_Game_State, HandleResetStateEvent);
        eventModuel.UnRegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleEntityHitBall);
    }

    private void HandleResetStateEvent(GameEvent eve)
    {
        if (m_aiInstance != null)
        {
            m_aiInstance.ResetStartPosition();

            AIStateController stateController = m_aiInstance.GetComponent<AIStateController>();
            if (stateController != null)
            {
                stateController.SwitchState(EAIStateType.Idle);
            }
        }
        
        GameDataModuel dataModuel = GameStart.GetInstance().DataModuel;
        if (dataModuel.m_heart <= 0)
        {
            if (SwitchStateAction != null)
            {
                SwitchStateAction(EGameStateType.GameTestState);
            }

            GameStart.GetInstance().LogModuel.Log(ELogType.Error, "You Died!!");
            return;
        }
        
        if (m_ball != null)
        {
            m_ball.Reset();
        }

        if (m_entityInstance != null)
        {
            m_entityInstance.ResetStartPosition();
        }
    }

    private void HandleEntityHitBall(GameEvent eve)
    {
        GameDataModuel dataModuel = GameStart.GetInstance().DataModuel;
        dataModuel.AddHitCount();
    }
}
