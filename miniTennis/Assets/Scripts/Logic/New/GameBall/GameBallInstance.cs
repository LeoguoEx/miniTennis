using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallInstance : MonoBehaviour
{
	private SpriteRenderer m_sprite;
	private Rigidbody2D m_rigidBody;
    private ParticleSystem m_particle;
    private GameBallCollider m_ballCollider;

    private float m_force;
    private Vector3 m_dir;
	
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

	    GameObject particle = CommonFunc.GetChild(gameObject, "Particle System");
	    if (particle != null)
	    {
	        m_particle = particle.GetComponent<ParticleSystem>();
        }

	    SetParticleActive(false);
	}

	public void SetVelocity(Vector2 dir, float force)
	{
	    m_force = force;
	    m_dir = dir;

        if (m_rigidBody != null)
		{
		    m_rigidBody.velocity = dir * force;
		}
	}

    public void SetPosition(Vector2 pos)
    {
        gameObject.transform.position = pos;
    }

    private void SetParticleActive(bool active)
    {
        if (m_particle != null)
        {
            m_particle.gameObject.SetActive(active);
        }
    }

    private void CollisionEnter2D(Collision2D other)
    {
        if(other == null) { return; }
        ContactPoint2D contactPoint = other.contacts[0];
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
}
