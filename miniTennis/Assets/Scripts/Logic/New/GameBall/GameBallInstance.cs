using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallInstance : MonoBehaviour
{
	private SpriteRenderer m_sprite;
	private Rigidbody2D m_rigidBody;
	
	void Start ()
	{
		GameObject ball = CommonFunc.GetChild(gameObject, "Ball");
		if (ball != null)
		{
			m_rigidBody = ball.GetComponent<Rigidbody2D>();
			m_sprite = ball.GetComponent<SpriteRenderer>();
		}
	}

	public void SetVelocity(Vector2 v)
	{
		if (m_rigidBody != null)
		{
			m_rigidBody.velocity = v;
		}
	}
}
