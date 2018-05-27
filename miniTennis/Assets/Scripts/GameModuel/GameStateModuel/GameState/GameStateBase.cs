using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameStateType
{
    GameMenuState = 0,
    GameExerciseState = 1,
    GameContestState = 2,
    GameTestState = 3,
    BombState = 4,
}

public abstract class GameStateBase 
{
    public EGameStateType StateType
    {
        get;
        protected set;
    }

    public Action<EGameStateType> SwitchStateAction;
    
    public GameStateBase(EGameStateType stateType)
    {
        StateType = stateType;
    }

    public virtual void PreEnterState()
    {
        
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();

    public virtual void PreExitState()
    {
        
    }

    public abstract void ExitState();
}
