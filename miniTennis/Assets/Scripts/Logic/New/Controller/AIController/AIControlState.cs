using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAIControlState
{
	Idle = 0,				//静止等待
	ChasingBall = 1,		//追求
	Prepare = 2,			//准备击球
	Hit = 3,				//击球
	FireBall= 4,			//开球
	
	None = 5,
}

public abstract class AIControlState 
{
	private EAIControlState m_statetype;
	public EAIControlState StateType
	{
		get;
		protected set;
	}

	protected Player m_player;
    
	public AIControlState(EAIControlState stateType, Player player)
	{
		m_statetype = stateType;
		m_player = player;
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
