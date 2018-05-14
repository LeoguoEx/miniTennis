﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameStateType
{
    GameMenuState = 0,
    GameExerciseState = 1,
    GameContestState = 2,
    GameTestState = 3,
}

public abstract class GameStateBase 
{
    public EGameStateType StateType
    {
        get;
        protected set;
    }
    
    public GameStateBase(EGameStateType stateType)
    {
        
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