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

    private List<GameBall> m_ballList;
    private List<GameBall> m_cacheBallList;

    public GameExerciseState(EGameStateType stateType) : base(stateType)
	{

	}

	public override void EnterState()
	{
	    m_ballList = new List<GameBall>();
        m_cacheBallList = new List<GameBall>();

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
        m_ballMechine = new BallMechine(mechineData);
	}

	public override void UpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	private void HitBallDelegate(Vector2 direction, float force)
	{
        GameBall[] balls = GetInPlayerAreaBalls();
        if(balls == null || balls.Length == 0) { return; }
	    for (int i = 0; i < balls.Length; i++)
	    {
	        balls[i].SetVelocity(direction, force);

        }
	}

    private GameBall CreateGameBall()
    {
        GameBall ball = null;
        if (m_cacheBallList.Count > 0)
        {
            ball = m_cacheBallList[0];
            m_cacheBallList.RemoveAt(0);
        }
        else
        {
            ball = new GameBall();
        }
        m_ballList.Add(ball);
        return ball;
    }

    private List<GameBall> m_checkBalls;
    private GameBall[] GetInPlayerAreaBalls()
    {
        if(m_checkBalls == null) { m_checkBalls = new List<GameBall>();}
        m_checkBalls.Clear();
        for (int i = 0; i < m_ballList.Count; i++)
        {
            GameBall ball = m_ballList[i];
            bool inarea = PlayerCollider.CheckInHitBallArea(ball.GetBallInstance().transform, m_palyer.Transform,
                m_palyer.PlayerData.m_radius, m_palyer.PlayerData.m_angle);
            if (inarea)
            {
                m_checkBalls.Add(ball);
            }
        }
        return m_checkBalls.ToArray();
    }
}
