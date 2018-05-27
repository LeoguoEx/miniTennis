using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAIControlState
{
	Idle = 0,				//静止等待
	ChasingBall = 1,		//追求
	Hit = 2,				//击球
	FireBall= 3,			//开球
    BackToBornPoint = 4,    //回到起始点
	
	None = 5,
}

public abstract class AIControlState 
{
	protected EAIControlState m_statetype;
	public EAIControlState StateType
	{
        get { return m_statetype; }
		protected set { m_statetype = value; }
	}

	protected Player m_player;

    public static Action<EAIControlState> SwitchStateAction;
    
	public AIControlState(Player player)
	{
		m_player = player;
	}

	public virtual void PreEnterState()
	{
        
	}

    public virtual void RegisterEvent()
    {
    }

    public virtual void UnRegisterEvent()
    {
    }
    
	public abstract void EnterState();

	public abstract void UpdateState(Transform target);

	public virtual void PreExitState()
	{
        
	}

	public abstract void ExitState();
}
