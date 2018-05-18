using UnityEngine;

public class PlayerController : PlayerControllerBase
{
	private bool m_fingureDown;
	private Vector2 m_fingerDownPosition;
	private Vector2 m_playerPosition;
	
	public override void InitController(Player player)
	{
		m_player = player;

		gameObject.AddComponent<FingerDownDetector>();
		gameObject.AddComponent<FingerUpDetector>();
		gameObject.AddComponent<FingerMotionDetector>();
	}

	public override void DestroyController()
	{
		GameObject.Destroy(this);
	}
	
	void OnFingerDown( FingerDownEvent e )
	{
		m_fingureDown = true;
		m_fingerDownPosition = e.Position;
		
		if (m_player != null)
		{
			m_playerPosition = m_player.GetPlayerPosition();
			m_player.StartMove();
		}
	}
	
	void OnFingerMove( FingerMotionEvent e )
	{
		if (m_player != null)
		{
			Vector2 fingerDir = e.Position - m_fingerDownPosition;
			float moveSpeed = m_player.GetPlayerMoveSpeed();
			Vector2 entityMovePosition = m_playerPosition + new Vector2(fingerDir.x, fingerDir.y) * moveSpeed;
			m_player.SetMoveDirection(fingerDir);
			m_player.MovePosition(entityMovePosition);
		}
	}

	void OnFingerStationary(FingerMotionEvent e)
	{
		if (m_player != null)
		{
			m_playerPosition = m_player.GetPlayerPosition();
		}
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		m_fingureDown = false;
	}
}
