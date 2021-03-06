﻿using System.Collections;
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
		
		m_changeAudio = true;
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
	}

	public int GetHeart()
	{
		return m_heart;
	}
}

public class GameContestState : GameStateBase
{
	private Player m_player;
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

    private bool m_change;
	
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
		m_player = new Player(1, playerData);
		m_player.InitPlayerAction(HitBallDelegate);
		
		GameObject go = new GameObject("Controller");
		m_playerController = go.AddComponent<PlayerController>();
		m_playerController.InitController(m_player);
		
		BallData ballData = new BallData();
		m_gameBall = new GameBall(ballData, m_ground.BounceLine);
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
        List<string> list = new List<string>
        {
            "BGM_001",
            "BGM_002",
            "lerp",
        };
	    audioModuel.PreLoadAudio(list);
        audioModuel.StopAudio();

	    m_player.Target = m_gameBall.GetBallInstance().transform;
	    m_ai.Target = m_gameBall.GetBallInstance().transform;
	    m_change = false;
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

		if (m_player != null)
		{
			m_player.Destroy();
			m_player = null;
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
		    CameraControl.GetInstance().TriggerMask();

		    GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
		    if (id == m_player.ID)
		    {
			    GameEventModuel meoduel = GameStart.GetInstance().EventModuel;
			    meoduel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f);
			    
			    m_side = ESide.Player;
			    m_contestData.AddIndex();
			    
			    m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
			    if (m_contestData != null && m_contestData.m_changeAudio && !m_change)
			    {
                    List<string> list = new List<string>
                    {
                        "lerp",
                        "BGM_002",
                    };
				    audioModuel.PlayBgAudio(list);
			        m_change = true;

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
			    ESide side = (id == m_player.ID) ? ESide.Player : ESide.AI;
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

		if (m_contestData != null && !m_contestData.m_changeAudio && m_change)
		{
//			GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
//			audioModuel.StopAudio();
		}

		m_contestUI.FreshUI(m_contestData.m_heart, m_contestData.m_index);
		m_gameBall.ResetVelocity();
		m_gameBall.SetPosition(m_ground.GroundData.GetFireBallPoint(ESide.Player));
		m_aiController.SwitchState(EAIControlState.BackToBornPoint);
		if (m_contestData.m_heart < 0)
		{
			m_player.SetIdle();
			m_contestUI.GameEnd();
			m_aiController.gameObject.SetActive(false);
			
			GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
			audioModuel.PlayAudio("lose");
		}
	}

	private int m_index = 0;
	private void OnTriggerEffectStart(GameEvent eve)
	{
		List<EEffectType> types = CreateRandomEffectType();
		EEffectType type = types[m_index];
		//TODO:触发执行对应的事件

		type = EEffectType.InkEffect;

		EffectBase effect = m_effect.GetEffectData<EffectBase>(type);
		if (effect != null)
		{
			m_ground.ExcuteEffect(effect, m_side);
			m_gameBall.ExcuteEffect(effect, m_side);
			m_contestUI.PlayEffect(effect, m_side);
		}

		m_index++;
	}

	private void OnTriggerEffectEnd(GameEvent eve)
	{
		if (m_effect != null)
		{
			m_effect.SetEffectEnd();
		}
	}

	private List<EEffectType> m_effectList;
	private List<EEffectType> CreateRandomEffectType()
	{
		bool random = false;
		if (m_effectList == null)
		{
			m_effectList = new List<EEffectType>();
			random = true;
		}

		if (!random && m_effectList.Count <= m_index)
		{
			m_index = 0;
			random = true;
		}

		if (random)
		{
			m_effectList.Clear();
			for (EEffectType i = EEffectType.Shield; i < EEffectType.MaxType; i++)
			{
				m_effectList.Add(i);
			}

			m_effectList.Sort((type, effectType) =>
			{
				float value = Random.Range(0, 1);
				if (value < 0.5f)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			});
		}
		
		return m_effectList;
	}
}
