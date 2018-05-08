using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestState : GameStateBase
{
    private Ball m_ball;
    private EntityInstance m_entityInstance;
    
    public GameTestState(EGameStateType stateType) 
        : base(stateType)
    {
        StateType = stateType;
    }

    public override void EnterState()
    {
        GameObject ball = GameObject.Find("Ball");
        if (ball != null)
        {
            m_ball = CommonFunc.AddSingleComponent<Ball>(ball);
        }
        
        GameObject chracter  =GameObject.Find("Character");
        if (chracter != null)
        {
            m_entityInstance = chracter.GetComponent<EntityInstance>();
        }
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        
    }
}
