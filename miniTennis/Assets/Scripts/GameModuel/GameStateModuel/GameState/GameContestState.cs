using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContestData
{
	public int m_index;
	public int m_heart;

	private int m_maxHeart;
	
	public GameContestData()
	{
		m_index = 0;
		m_heart = 3;

		m_maxHeart = 5;
	}

	public void AddIndex()
	{
		m_index++;
	}

	public void AddHeart()
	{
		m_heart++;
		m_heart = Mathf.Clamp(m_heart, 0, m_maxHeart);
	}

	public void ReduceHeart()
	{
		m_heart--;
	}

	public int GetHeart()
	{
		return m_heart;
	}
}

public class GameContestState : GameStateBase
{
	private Player m_palyer;
	private PlayerController m_playerController;
	private GameBall m_gameBall;
	private Ground m_ground;

	private Player m_ai;
	private AIController m_aiController;

	private GameContestData m_contestData;

	private GameContestUI m_contestUI;

	public GameContestState(EGameStateType stateType) : base(stateType)
	{
		
	}

	public override void EnterState()
	{
		m_contestData = new GameContestData();
		
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
		m_gameBall.SetOutofRangeAction(GameBallOutofRange);
        m_gameBall.SetPosition(groundData.GetFireBallPoint(ESeriveSide.Player));
		
		AIPlayerData aiData = new AIPlayerData();
		m_ai = new Player(2, aiData);
        m_ai.Transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		m_ai.InitPlayerAction(HitBallDelegate);

		m_aiController = go.AddComponent<AIController>();
	    m_aiController.SetGameBall(m_gameBall);
        m_aiController.InitController(m_ai);

		m_contestUI = GameStart.GetInstance().UIModuel.LoadResUI<GameContestUI>("ContestPrefab");
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

		if (m_ai != null)
		{
			m_ai.Destroy();
			m_ai = null;
		}

		if (m_aiController != null)
		{
			m_aiController.DestroyController();
			m_aiController = null;
		}
		
		GameStart.GetInstance().UIModuel.UnLoadResUI(m_contestUI.gameObject);
	}
	
	private void HitBallDelegate(Player player, Vector2 direction, float force, int id)
	{
		if(m_gameBall == null){return;}

	    bool checkIsHitArea = PlayerCollider.CheckInHitBallArea(m_gameBall.GetBallInstance().transform, player.Transform,
		    player.PlayerData.m_radius, player.PlayerData.m_angle);
	    if (checkIsHitArea)
	    {
	        if (m_gameBall != null)
	        {
	            m_gameBall.SetVelocity(direction, force);
	        }

	        GameEventModuel meoduel = GameStart.GetInstance().EventModuel;
	        meoduel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f, id);
		    
		    CameraControl.GetInstance().Trigger();

		    if (id == m_palyer.ID)
		    {
			    m_contestData.AddIndex();
			    m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
		    }
        }
	}

	private void GameBallOutofRange(GameBall ball)
	{
		Vector3 position = m_gameBall.GetPosition();
		if (position.y > 0)
		{
			m_contestData.AddHeart();
		}
		else
		{
			m_contestData.ReduceHeart();
		}

		m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
		m_gameBall.SetVelocity(Vector2.zero, 0f);
		m_gameBall.SetPosition(m_ground.GroundData.GetFireBallPoint(ESeriveSide.Player));
		m_aiController.SwitchState(EAIControlState.BackToBornPoint);
		if (m_contestData.m_heart < 0)
		{
			m_palyer.SetIdle();
			m_contestUI.GameEnd();
			m_aiController.gameObject.SetActive(false);
		}
	}
}
