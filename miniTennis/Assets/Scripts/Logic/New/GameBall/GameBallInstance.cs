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
    private GameObject m_trail;
    private GameBallCollider m_ballCollider;

    private float m_force;
    private Vector3 m_dir;
	private Vector3 m_preDir;
	private Rect m_getBallRect;
	private ContactPoint2D m_collision;

	private Animator m_animator;

	public Action BallOutofRangeAction = null;

    private float m_rotationSpeed;
    private float m_aTanTarget;
	private Vector3 m_newDir;

	private bool m_makeItOffset;
	private Vector2 m_offsetDir;
	private bool m_needOffset;

    private Action<string> m_bounceAction;
    private string m_colliderName;

    public Vector2 GetDir()
    {
        return m_dir;
    }

    public void SetBounceAction(Action<string> action)
    {
        m_bounceAction = action;
    }
	
	void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody2D>();
	    m_ballCollider = gameObject.AddComponent<GameBallCollider>();
	    m_ballCollider.m_colliderAction = CollisionEnter2D;
		m_animator = gameObject.GetComponent<Animator>();

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

	    m_trail = CommonFunc.GetChild(gameObject, "Trail");

	}

	void Update()
	{
		if (CheckOutOfRange() && BallOutofRangeAction != null)
		{
			BallOutofRangeAction();
		}

		if (m_rigidBody.velocity != Vector2.zero && m_needOffset)
		{
			Vector2 dir = m_rigidBody.velocity.normalized;
			m_rigidBody.velocity = Vector2.zero;
			dir = (m_offsetDir + dir);
			m_rigidBody.velocity =  dir* m_force;
			RotateToDirAngle(dir);
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
		
		m_animator.Play("Empty");
		m_animator.Play("Bounce");

		if (m_rigidBody != null)
		{
            transform.localScale = new Vector3(0.6f, 1f, 1f);
            
            RotateToDirAngle(dir);

            m_rigidBody.velocity = Vector2.zero;
			m_rigidBody.velocity = dir * force;
		}

		
//		if (m_quick != null && m_mid != null && m_low != null)
//		{
//			m_low.gameObject.SetActive((force < 10));
//			m_mid.gameObject.SetActive((force >= 10 && force < 13));
//			m_quick.gameObject.SetActive((force >= 13));
//		}
	    SetParticleActive(true);

	}

	public void SetOffsetDir(Vector2 dir)
	{
		m_offsetDir = dir;
	}

    public void SetNeedOffset(bool needOffset)
    {
        m_needOffset = needOffset;
    }

	public void FresetVelocity()
	{
		if (m_rigidBody != null)
		{
			gameObject.transform.localScale = Vector3.one;
			m_rigidBody.velocity = Vector2.zero;
			transform.rotation = Quaternion.identity;
		}
	}

    public void SetPosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
	    SetParticleActive(false);
    }

    public void PlayBounceUpAndDown()
    {
        if (m_animator != null)
        {
            m_animator.Play("BounceUpDown");
        }
    }

    private void SetParticleActive(bool active)
    {
        if (m_quick != null)
        {
	        m_quick.gameObject.SetActive(false);
        }
	    
	    if (m_mid != null)
	    {
		    m_mid.gameObject.SetActive(false);
	    }
	    
	    if (m_low != null)
	    {
		    m_low.gameObject.SetActive(false);
        }

        if (m_trail != null)
        {
            m_trail.gameObject.SetActive(active);

            TrailRenderer render = m_trail.GetComponent<TrailRenderer>();
            if (render != null)
            {
                render.Clear();
            }
        }
    }

	private bool m_bounce;
    private void CollisionEnter2D(Collision2D other)
    {
	    OnBounce(other);
    }

	private void OnCollisionStay2D(Collision2D other)
	{
		if (!m_bounce)
		{
			OnBounce(other);
		}
	}

	private void OnBounce(Collision2D other)
	{
		if(other == null) { return; }
		m_collision = other.contacts[0];
	    
		ContactPoint2D contactPoint = m_collision;
		Vector3 newDir = Vector3.Reflect(m_dir, contactPoint.normal);
		newDir.z = 0f;
		m_newDir = newDir;
	    
		m_animator.Play("Empty");
		m_animator.Play("Bounce");

		m_colliderName = other.gameObject.name;
		Bounce();
		m_bounce = true;
	}

	private void Bounce()
	{
	    if (m_bounceAction != null)
	    {
	        m_bounceAction(m_colliderName);
	    }

		m_offsetDir = new Vector2(-m_offsetDir.x, 0f);
        RotateToDirAngle(m_newDir);
        if (m_rigidBody != null)
		{
			m_rigidBody.velocity = Vector2.zero;
			m_rigidBody.velocity = m_newDir.normalized * m_force;
		}
		m_dir = m_newDir;
		
		transform.localScale = new Vector3(0.6f, 1f, 1f);
	}

    private void RotateToDirAngle(Vector3 dir)
    {
        float angle = Vector3.Angle(Vector3.up, dir.normalized);
        Vector3 newVector = Vector3.Cross(Vector3.up, dir.normalized);
        if (newVector.z < 0)
        {
            angle = -angle;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f , angle));
    }
}
