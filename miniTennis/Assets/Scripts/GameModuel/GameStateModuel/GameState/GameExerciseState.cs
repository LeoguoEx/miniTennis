using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExerciseState : GameStateBase
{
	private Player m_palyer;
	private PlayerController m_playerController;
	//private GameBall m_gameBall;
	private Ground m_ground;
    private BallMechine m_ballMechine;

	private float m_time = 0f;
	private float m_rate = 3f;

    public GameExerciseState(EGameStateType stateType) : base(stateType)
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
		m_palyer = new Player(playerData);
		m_palyer.InitPlayerAction(HitBallDelegate);
		
		GameObject go = new GameObject("Controller");
		m_playerController = go.AddComponent<PlayerController>();
		m_playerController.InitController(m_palyer);
		
        BallMechineData mechineData = new BallMechineData();
		BallData ballData = new BallData();
        m_ballMechine = new BallMechine(mechineData, ballData, m_ground.GetLeftPoint(), m_ground.GetRightPoint());
		
		CoroutineTool.GetInstance().StartGameCoroutine(StartCoroutine());
	}

	private IEnumerator StartCoroutine()
	{
		yield return new WaitForSeconds(3f);

		if (m_ballMechine != null)
		{
			m_ballMechine.StartEvent();
		}
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

		if (m_ballMechine != null)
		{
			m_ballMechine.Destory();
			m_ballMechine = null;
		}
	}

	private void HitBallDelegate(Vector2 direction, float force)
	{
		if(m_ballMechine == null){return;}
        GameBall[] balls = m_ballMechine.GetInPlayerAreaBalls(m_palyer);
        if(balls == null || balls.Length == 0) { return; }
	    for (int i = 0; i < balls.Length; i++)
	    {
	        balls[i].SetVelocity(direction, force);

        }
	}
}
