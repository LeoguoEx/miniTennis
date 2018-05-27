using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBombState : GameStateBase
{
    private Player m_player;
    private PlayerController m_playerController;
    private BombBall m_bombBall;
    private Ground m_ground;
    private GameEffect m_effect;

    private Player m_ai;
    private AIController m_aiController;

    private GameContestData m_contestData;

    private GameBombUI m_bombUI;

    private List<string> m_audioNameList;
    private int m_playerIndex;
    private int m_aiIndex;
    private ESide m_side;

    private bool m_change;

    private float m_playerBombTime;
    private float m_aiBombTime;
    private float m_playerTotalBombTime;
    private float m_aiTotalBombTime;

    private bool m_start;

    public GameBombState(EGameStateType stateType)
        : base(stateType)
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
        m_bombBall = new BombBall(ballData, m_ground.BounceLine);
        m_bombBall.SetOutofRangeAction(GameBallOutofRange);
        m_bombBall.SetPosition(groundData.GetFireBallPoint(ESide.Player));

        AIPlayerData aiData = new AIPlayerData();
        m_ai = new Player(2, aiData);
        m_ai.Transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        m_ai.InitPlayerAction(HitBallDelegate);

        m_aiController = go.AddComponent<AIController>();
        m_aiController.SetBomBall(m_bombBall);
        m_aiController.InitController(m_ai);

        m_effect = new GameEffect();

        m_bombUI = GameStart.GetInstance().UIModuel.LoadResUI<GameBombUI>("BombUI");

        GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
        List<string> list = new List<string>
        {
            "BGM_001",
            "BGM_002",
            "lerp",
        };
        audioModuel.PreLoadAudio(list);
        audioModuel.StopAudio();

        m_player.Target = m_bombBall.GetBallInstance().transform;
        m_ai.Target = m_bombBall.GetBallInstance().transform;

        m_playerBombTime = 0f;
        m_aiBombTime = 0f;
        m_playerTotalBombTime = 20f;
        m_aiTotalBombTime = 20f;
    }

    public override void UpdateState()
    {
        if (m_effect != null)
        {
            m_effect.Update();
        }

        if (m_bombBall != null)
        {
            m_bombBall.Update();
        }

        HandleTime();

        if (m_bombUI != null)
        {
            m_bombUI.SetPlayerLastTime(m_playerTotalBombTime - m_playerBombTime);
            m_bombUI.SetAiLastTime(m_aiTotalBombTime - m_aiBombTime);
        }
    }

    private void HandleTime()
    {
        if (m_bombBall != null && m_start)
        {
            Vector3 position = m_bombBall.GetPosition();
            if (position.y > 0)
            {
                m_aiBombTime += Time.deltaTime;
                if (m_aiBombTime > m_aiTotalBombTime)
                {
                    m_bombBall.PlayBomb();
                }
                float value = m_aiBombTime / m_aiTotalBombTime;
                m_bombBall.SetBombScale(value);
            }
            else
            {
                m_playerBombTime += Time.deltaTime;
                if (m_playerBombTime > m_playerTotalBombTime)
                {
                    m_bombBall.PlayBomb();
                }
                float value = m_playerBombTime / m_playerTotalBombTime;
                m_bombBall.SetBombScale(value);
            }
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

        if (m_bombBall != null)
        {
            m_bombBall.Destory();
            m_bombBall = null;
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

        GameStart.GetInstance().UIModuel.UnLoadResUI(m_bombUI.gameObject);
    }

    private void HitBallDelegate(Player player, Vector2 direction, float force, int id)
    {
        m_start = true;
        if (m_bombBall == null) { return; }

        bool checkIsHitArea = PlayerCollider.CheckInHitBallArea(m_bombBall.GetBallInstance().transform, player.Transform,
            player.PlayerData.m_radius, player.PlayerData.m_angle, player.BoxCollider);
        if (checkIsHitArea)
        {
            CameraControl.GetInstance().Trigger();

            GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
            if (id == m_player.ID)
            {
                GameEventModuel meoduel = GameStart.GetInstance().EventModuel;
                meoduel.SendEvent(GameEventID.PLAYER_HIT_BALL, true, 0f);

                m_side = ESide.Player;
                m_contestData.AddIndex();
                
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

            if (m_bombBall != null)
            {
                ESide side = (id == m_player.ID) ? ESide.Player : ESide.AI;
                m_bombBall.SetVelocity(direction, force, side);
                m_bombBall.ChangeSide(side);
            }
        }
    }

    private void GameBallOutofRange(BombBall ball)
    {
        m_start = false;
        Vector3 position = m_bombBall.GetPosition();
        if (position.y > 0)
        {
            m_contestData.AddHeart();
            m_aiIndex = 0;
            m_aiBombTime += 2f;
        }
        else
        {
            m_contestData.ReduceHeart();
            m_playerIndex = 0;
            m_playerBombTime += 2f;
        }

        if (m_contestData != null && !m_contestData.m_changeAudio && m_change)
        {
            GameAudioModuel audioModuel = GameStart.GetInstance().AudioModuel;
            audioModuel.StopAudio();
        }
        
        m_bombBall.ResetVelocity();
        m_bombBall.SetPosition(m_ground.GroundData.GetFireBallPoint(ESide.Player));
        m_aiController.SwitchState(EAIControlState.BackToBornPoint);
        if (m_contestData.m_heart < 0)
        {
            m_player.SetIdle();
            m_aiController.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEffectStart(GameEvent eve)
    {
//        EEffectType[] types = CreateRandomEffectType();
//        int random = Random.Range(0, types.Length);
//        if (types.Length > 0)
//        {
//            EEffectType type = types[random];
//            //TODO:触发执行对应的事件
//
//            //type = EEffectType.BananaBall;
//
//            EffectBase effect = m_effect.GetEffectData<EffectBase>(type);
//            if (effect != null)
//            {
//                //m_ground.ExcuteEffect(effect, m_side);
//                //m_bombBall.ExcuteEffect(effect, m_side);
//            }
//        }
        if (m_side == ESide.Player)
        {
            m_playerTotalBombTime += 5f;
        }
        else if (m_side == ESide.AI)
        {
            m_aiTotalBombTime += 5f;
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
