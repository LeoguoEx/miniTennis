using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContestData
{
	public int m_index;
	public int m_heart;

	private int m_maxHeart;

	public bool m_changeAudio;
	private int m_recordCount;
	
	public GameContestData()
	{
		m_index = 0;
		m_heart = 3;

		m_maxHeart = 5;
	}

	public void AddIndex()
	{
		m_index++;
		m_recordCount++;
		if (m_recordCount > 5)
		{
			m_changeAudio = true;
		}
	}

	public void AddHeart()
	{
		m_heart++;
		m_heart = Mathf.Clamp(m_heart, 0, m_maxHeart);
	}

	public void ReduceHeart()
	{
		m_heart--;
		m_recordCount = 0;
		m_changeAudio = false;
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
	private GameEffect m_effect;

	private Player m_ai;
	private AIController m_aiController;

	private GameContestData m_contestData;

	private GameContestUI m_contestUI;

	private List<string> m_audioNameList;
	private int m_playerIndex;
	private int m_aiIndex;
	private ESide m_side;
	
	public GameContestState(EGameStateType stateType) : base(stateType)
	{
		m_audioNameList = new List<string>
		{
			"01",
			"02",
			"03",
			"04",
			"05"
		};
	}

	public override void EnterState()
	{
		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.RegisterEventListener(GameEventID.TRIGGER_GAME_EVENT, OnTriggerEffectStart);
		eventModuel.RegisterEventListener(GameEventID.END_GAME_EVENT, OnTriggerEffectEnd);
		
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
        m_gameBall.SetPosition(groundData.GetFireBallPoint(ESide.Player));
		
		AIPlayerData aiData = new AIPlayerData();
		m_ai = new Player(2, aiData);
        m_ai.Transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		m_ai.InitPlayerAction(HitBallDelegate);

		m_aiController = go.AddComponent<AIController>();
	    m_aiController.SetGameBall(m_gameBall);
        m_aiController.InitController(m_ai);
		
		m_effect = new GameEffect();

		m_contestUI = GameStart.GetInstance().UIModuel.LoadResUI<GameContestUI>("ContestPrefab");
		CoroutineTool.GetInstance().StartCoroutine(SetUI());
		
		GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
		audioModuel.PlayBgAudio("BGM_001");
	}

	private IEnumerator SetUI()
	{
		yield return new WaitForEndOfFrame();
		m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
	}

	public override void UpdateState()
	{
		if (m_effect != null)
		{
			m_effect.Update();
		}

		if (m_gameBall != null)
		{
			m_gameBall.Update();
		}
	}

	public override void ExitState()
	{
		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.UnRegisterEventListener(GameEventID.TRIGGER_GAME_EVENT, OnTriggerEffectStart);
		eventModuel.UnRegisterEventListener(GameEventID.END_GAME_EVENT, OnTriggerEffectEnd);
		
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

		if (m_effect != null)
		{
			m_effect.Destory();
		}
		
		GameStart.GetInstance().UIModuel.UnLoadResUI(m_contestUI.gameObject);
	}
	
	private void HitBallDelegate(Player player, Vector2 direction, float force, int id)
	{
		if(m_gameBall == null){return;}

	    bool checkIsHitArea = PlayerCollider.CheckInHitBallArea(m_gameBall.GetBallInstance().transform, player.Transform,
		    player.PlayerData.m_radius, player.PlayerData.m_angle, player.BoxCollider);
	    if (checkIsHitArea)
	    {
		    CameraControl.GetInstance().Trigger();

		    GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
		    if (id == m_palyer.ID)
		    {
			    GameEventModuel meoduel = GameStart.GetInstance().EventModuel;
			    meoduel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f);
			    
			    m_side = ESide.Player;
			    m_contestData.AddIndex();
			    
			    m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
			    if (m_contestData != null && m_contestData.m_changeAudio)
			    {
				    audioModuel.PlayBgAudio("BGM_002");
			    }

			    m_playerIndex++;
			    m_playerIndex = Mathf.Clamp(m_playerIndex, 0, m_audioNameList.Count - 1);
			    audioModuel.PlayAudio(m_audioNameList[m_playerIndex]);
		    }
		    else
		    {
			    m_side = ESide.AI;
			    m_aiIndex++;
			    m_aiIndex = Mathf.Clamp(m_aiIndex, 0, m_audioNameList.Count - 1);
			    audioModuel.PlayAudio(m_audioNameList[m_aiIndex]);
		    }

		    if (m_gameBall != null)
		    {
			    ESide side = (id == m_palyer.ID) ? ESide.Player : ESide.AI;
			    m_gameBall.SetVelocity(direction, force, side);
			    m_gameBall.ChangeEffectDir(side);
		    }
	    }
	}

	private void GameBallOutofRange(GameBall ball)
	{
		Vector3 position = m_gameBall.GetPosition();
		if (position.y > 0)
		{
			m_contestData.AddHeart();
			m_aiIndex = 0;
		}
		else
		{
			m_contestData.ReduceHeart();
			m_playerIndex = 0;
		}

		if (m_contestData != null && !m_contestData.m_changeAudio)
		{
			GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
			audioModuel.PlayBgAudio("BGM_001");
		}

		m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
		m_gameBall.ResetVelocity();
		m_gameBall.SetPosition(m_ground.GroundData.GetFireBallPoint(ESide.Player));
		m_aiController.SwitchState(EAIControlState.BackToBornPoint);
		if (m_contestData.m_heart < 0)
		{
			m_palyer.SetIdle();
			m_contestUI.GameEnd();
			m_aiController.gameObject.SetActive(false);
		}
	}
	
	
	private void OnTriggerEffectStart(GameEvent eve)
	{
		EEffectType[] types = CreateRandomEffectType();
		int random = Random.Range(0, types.Length);
		if (types.Length > 0)
		{
			EEffectType type = types[random];
			//TODO:触发执行对应的事件

			//type = EEffectType.BananaBall;

			EffectBase effect = m_effect.GetEffectData<EffectBase>(type);
			if (effect != null)
			{
				m_ground.ExcuteEffect(effect, m_side);
				m_gameBall.ExcuteEffect(effect, m_side);
			}
		}
	}

	private void OnTriggerEffectEnd(GameEvent eve)
	{
		if (m_effect != null)
		{
			m_effect.SetEffectEnd();
		}
	}

	private EEffectType[] CreateRandomEffectType()
	{
		int count = (int)EEffectType.MaxType;
		if (m_effect.PreEffectType != EEffectType.MaxType)
		{
			count--;
		}

		EEffectType[] types = new EEffectType[count];
		int index = 0;
		for (EEffectType i = 0; i < EEffectType.MaxType; i++)
		{
			if (m_effect.PreEffectType == i)
			{
				continue;
			}

			types[index] = i;
		}

		return types;
	}
}
