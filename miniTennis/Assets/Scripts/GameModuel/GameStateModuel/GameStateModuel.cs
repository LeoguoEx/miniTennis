﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateModuel : GameModuelBase
{
	private Dictionary<int, GameStateBase> m_stateDic;
	private GameStateBase m_curState;
	private EGameStateType m_curStateType;
	
	public override void Init()
	{
		m_stateDic = new Dictionary<int, GameStateBase>();
		m_stateDic.Add((int)EGameStateType.GameTestState, new GameTestState(EGameStateType.GameTestState));
		m_stateDic.Add((int)EGameStateType.GameExerciseState, new GameExerciseState(EGameStateType.GameExerciseState));
		m_stateDic.Add((int)EGameStateType.GameContestState, new GameContestState(EGameStateType.GameContestState));
		m_stateDic.Add((int)EGameStateType.GameMenuState, new GameMenuState(EGameStateType.GameMenuState));
	    m_stateDic.Add((int)EGameStateType.BombState, new GameBombState(EGameStateType.BombState));

        foreach (int id in m_stateDic.Keys)
		{
			m_stateDic[id].SwitchStateAction = SwitchState;
		}
		
		SwitchState(EGameStateType.GameMenuState);

		GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
		eveModuel.RegisterEventListener(GameEventID.SWITCH_GAME_STATE, SwitchState);
	}

	public void SwitchState(EGameStateType stateType)
	{
		if (m_curState != null)
		{
			m_curState.PreExitState();
			m_curState.ExitState();
		}

		m_stateDic.TryGetValue((int) stateType, out m_curState);
		if (m_curState != null && m_curState.StateType == stateType)
		{
			m_curState.PreEnterState();
			m_curState.EnterState();
		}
	}

	private void Update()
	{
		if (m_curState != null)
		{
			m_curState.UpdateState();
		}
	}

	private void SwitchState(GameEvent eve)
	{
		EGameStateType statetype = eve.GetParamByIndex<EGameStateType>(0);
		SwitchState(statetype);
	}
}
