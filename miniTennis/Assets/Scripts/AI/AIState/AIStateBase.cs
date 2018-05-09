using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAIStateType
{
	Idle = 0,				//静止等待
	GotoHitTarget = 1,		//追求
	Prepare = 2,			//准备击球
	Hit = 3,				//击球
	FireBall= 4,			//开球
	
	None = 5,
}

public abstract class AIStateBase 
{
	public EAIStateType StateType
	{
		get;
		protected set;
	}
    
	public AIStateBase(EAIStateType stateType)
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
