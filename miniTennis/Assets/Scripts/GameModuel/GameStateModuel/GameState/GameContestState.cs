using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContestState : GameStateBase
{
	private Player m_palyer;
	private PlayerController m_playerController;
	private GameBall m_gameBall;
	private Ground m_ground;

	private Player m_ai;
	private AIController m_aiController;

	public GameContestState(EGameStateType stateType) : base(stateType)
	{
		
	}

	public override void EnterState()
	{
		GameObject ground = GameStart.GetInstance().ResModuel.LoadResources<GameObject>(EResourceType.Ground, "Ground");
		ground = CommonFunc.Instantiate(ground);
		m_ground = CommonFunc.AddSingleComponent<Ground>(ground);
		GroundData groundData = new GroundData();
		m_ground.InitGround(groundData);
		
		PlayerData playerData = new PlayerData();
		m_palyer = new Player(1, playerData);
		m_palyer.InitPlayerAction(HitBallDelegate);
		
		GameObject go = new GameObject("Controller");
		m_playerController = go.AddComponent<PlayerController>();
		m_playerController.InitController(m_palyer);
		
		BallData ballData = new BallData();
		m_gameBall = new GameBall(ballData);
        m_gameBall.SetPosition(groundData.GetFireBallPoint(ESeriveSide.Player));
		
		AIPlayerData aiData = new AIPlayerData();
		m_ai = new Player(2, aiData);
        m_ai.Transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		m_ai.InitPlayerAction(HitBallDelegate);

		m_aiController = go.AddComponent<AIController>();
	    m_aiController.SetGameBall(m_gameBall);
        m_aiController.InitController(m_ai);
	}

	public override void UpdateState()
	{
		
	}

	public override void ExitState()
	{
		if (m_ground != null)
		{
			GameObject.Destroy(m_ground.gameObject);
			m_ground = null;
		}

		if (m_palyer != null)
		{
			m_palyer.Destroy();
			m_palyer = null;
		}

		if (m_playerController != null)
		{
			m_playerController.DestroyController();
			m_playerController = null;
		}

		if (m_gameBall != null)
		{
			m_gameBall.Destory();
			m_gameBall = null;
		}

		if (m_aiController != null)
		{
			m_aiController.DestroyController();
			m_aiController = null;
		}
	}
	
	private void HitBallDelegate(Vector2 direction, float force, int id)
	{
		if(m_gameBall == null){return;}

	    bool checkIsHitArea = PlayerCollider.CheckInHitBallArea(m_gameBall.GetBallInstance().transform, m_palyer.Transform,
	        m_palyer.PlayerData.m_radius, m_palyer.PlayerData.m_angle);
	    if (checkIsHitArea)
	    {
	        if (m_gameBall != null)
	        {
	            m_gameBall.SetVelocity(direction, force);
	        }

	        GameEventModuel meoduel = GameStart.GetInstance().EventModuel;
	        meoduel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f, id);
        }
	}
}
