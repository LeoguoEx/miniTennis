using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuState : GameStateBase
{
	private GameStartUI m_startUI;
	
	public GameMenuState(EGameStateType stateType) : base(stateType)
	{
		
		
	}

	public override void EnterState()
	{
		m_startUI = GameStart.GetInstance().UIModuel.LoadResUI<GameStartUI>("StartUI");
		GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
		moduel.PlayBgAudio("BGM_001");
	}

	public override void UpdateState()
	{
	}

	public override void ExitState()
	{
		GameStart.GetInstance().UIModuel.UnLoadResUI(m_startUI.gameObject);
	}
}
