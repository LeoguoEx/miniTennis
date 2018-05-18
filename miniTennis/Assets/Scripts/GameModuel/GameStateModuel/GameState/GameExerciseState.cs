using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExerciseState : GameStateBase
{
	private Player m_palyer;
	private PlayerController m_playerController;
	private GameBall m_gameBall;
	private Ground m_ground;
	

	public GameExerciseState(EGameStateType stateType) : base(stateType)
	{
	}

	public override void EnterState()
	{
		GameObject ground = GameStart.GetInstance().ResModuel.LoadResources<GameObject>(EResourceType.UI)
		
		
		PlayerData data = new PlayerData();
		m_palyer = new Player(data);
		m_palyer.InitPlayerAction(GetCurPlayerTarget, HitBallDelegate);
		
		GameObject go = new GameObject("Controller");
		m_playerController = go.AddComponent<PlayerController>();
		m_playerController.InitController(m_palyer);
		
		m_gameBall = new GameBall();
	}

	public override void UpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}
	
	private GameObject GetCurPlayerTarget()
	{
		return m_gameBall.GetBallInstance();
	}

	private void HitBallDelegate(Vector2 direction, float force)
	{
		m_gameBall.SetVelocity(direction * force);
	}
}
