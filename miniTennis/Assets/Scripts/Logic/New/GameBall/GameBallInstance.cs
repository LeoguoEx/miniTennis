using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallInstance : MonoBehaviour
{
	private SpriteRenderer m_sprite;
	private Rigidbody2D m_rigidBody;
	private ParticleSystem m_quick;
	private ParticleSystem m_mid;
	private ParticleSystem m_low;
    private GameBallCollider m_ballCollider;

    private float m_force;
    private Vector3 m_dir;
	private Vector3 m_preDir;
	private Rect m_getBallRect;
	private ContactPoint2D m_collision;

	public Action BallOutofRangeAction = null;
	
	void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody2D>();
	    m_ballCollider = gameObject.AddComponent<GameBallCollider>();
	    m_ballCollider.m_colliderAction = CollisionEnter2D;

        GameObject ball = CommonFunc.GetChild(gameObject, "Ball");
		if (ball != null)
		{
		    m_sprite = GetComponent<SpriteRenderer>();
		}

	    GameObject particle = CommonFunc.GetChild(gameObject, "Quick");
	    if (particle != null)
	    {
	        m_quick = particle.GetComponent<ParticleSystem>();
		    m_quick.gameObject.SetActive(false);
	    }
		
		particle = CommonFunc.GetChild(gameObject, "Mid");
		if (particle != null)
		{
			m_mid = particle.GetComponent<ParticleSystem>();
			m_mid.gameObject.SetActive(false);
		}
		
		particle = CommonFunc.GetChild(gameObject, "Low");
		if (particle != null)
		{
			m_low = particle.GetComponent<ParticleSystem>();
			m_low.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if (CheckOutOfRange() && BallOutofRangeAction != null)
		{
			BallOutofRangeAction();
		}
	}

	public void SetBallRect(Rect rect)
	{
		m_getBallRect = rect;
	}

	private bool CheckOutOfRange()
	{
		Vector3 position = transform.position;
		if (position.x < m_getBallRect.x || position.x > m_getBallRect.width ||
		    position.y < m_getBallRect.y || position.y > m_getBallRect.height)
		{
			return true;
		}
		return false;
	}

	public void SetVelocity(Vector2 dir, float force)
	{
	    m_force = force;
	    m_dir = dir;

		if (m_rigidBody != null)
		{
			m_rigidBody.velocity = Vector2.zero;
		}

		StartCoroutine(DelaySetVelocity(dir, force));
	}

	public void FresetVelocity()
	{
		if (m_rigidBody != null)
		{
			m_rigidBody.velocity = Vector2.zero;
			transform.localScale = Vector3.one;
			transform.rotation = Quaternion.identity;
		}
	}

    public void SetPosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
	    gameObject.transform.localScale = Vector3.one;
	    SetParticleActive(false);
    }

    private void SetParticleActive(bool active)
    {
        if (m_quick != null)
        {
	        m_quick.gameObject.SetActive(active);
        }
	    
	    if (m_mid != null)
	    {
		    m_mid.gameObject.SetActive(active);
	    }
	    
	    if (m_low != null)
	    {
		    m_low.gameObject.SetActive(active);
	    }
    }

    private void CollisionEnter2D(Collision2D other)
    {
        if(other == null) { return; }

	    m_rigidBody.velocity = Vector3.zero;
	    m_collision = other.contacts[0];
	    StartCoroutine(Bounce());
    }

	private IEnumerator Bounce()
	{
		yield return new WaitForSeconds(0.07f);
		ContactPoint2D contactPoint = m_collision;
		Vector3 newDir = Vector3.Reflect(m_dir, contactPoint.normal);
		newDir.z = 0f;
		Quaternion rotation = Quaternion.FromToRotation(m_dir, newDir);
		transform.rotation = rotation;
		if (m_rigidBody != null)
		{
			m_rigidBody.velocity = newDir.normalized * m_force;
		}
		m_dir = newDir;
	}

	private IEnumerator DelaySetVelocity(Vector2 dir, float force)
	{
		yield return new WaitForSeconds(0.07f);
		
		gameObject.transform.localScale= new Vector3(0.65f, gameObject.transform.localScale.y, 1f);

		if (m_rigidBody != null)
		{
			transform.rotation = Quaternion.FromToRotation(m_preDir, dir);
			m_rigidBody.velocity = Vector2.zero;
			m_rigidBody.velocity = dir * force;
		}

		if (m_quick != null && m_mid != null && m_low != null)
		{
			m_low.gameObject.SetActive((force < 10));
			m_mid.gameObject.SetActive((force >= 10 && force < 13));
			m_quick.gameObject.SetActive((force >= 13));
		}

		m_preDir = dir;
	}
}
