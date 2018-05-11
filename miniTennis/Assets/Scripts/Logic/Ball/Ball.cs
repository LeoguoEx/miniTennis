using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHitForceType
{
    Soft = 1,
    Middle = 2,
    High = 3,
}

public class Ball : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_sprite;

    [SerializeField]
    private Vector3 m_speedRate = new Vector3(5f, 10f, 20f);
    
    private Vector3 m_startPosition = new Vector3(-0f, 0.81f, 0f);
    
    
    [SerializeField]
    private Animator m_animator;

    private Rigidbody2D m_rigidBody;
    private float m_speed;

    private Vector3 m_dir;

    private ParticleSystem m_particle;

    public Vector3 Velocity
    {
        get
        {
            if (m_rigidBody != null)
            {
                return m_rigidBody.velocity;
            }
            return Vector3.zero;
        }
    }
    
    void Start()
    {
        GameBallManager.GetInstance().RegisterBall(this);
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_particle = GetComponentInChildren<ParticleSystem>();
        if (m_particle != null)
        {
            m_particle.gameObject.SetActive(false);
        }

        m_startPosition = gameObject.transform.position;
    }

    public void Reset()
    {
        gameObject.transform.position = m_startPosition;
        if (m_particle != null)
        {
            m_particle.gameObject.SetActive(false);
        }

        if (m_rigidBody != null)
        {
            m_rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.RegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleHitBallMessage);
        eventModuel.RegisterEventListener(GameEventID.AI_HIT_BALL, HandleHitBallMessage);
    }

    private void OnDisable()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.UnRegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleHitBallMessage);
        eventModuel.UnRegisterEventListener(GameEventID.AI_HIT_BALL, HandleHitBallMessage);
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    public void SetBallInfo(string ball)
    {
        if (m_sprite != null)
        {
            Sprite texture = GameStart.GetInstance().ResModuel.LoadResources<Sprite>(EResourceType.Ball, ball);
            m_sprite.sprite = texture;
        }
    }

    private void PlayAnim(string animName)
    {
        if (m_animator != null)
        {
            m_animator.Play(animName);
        }
    }

    private void HandleHitBallMessage(GameEvent eve)
    {
        if (eve != null)
        {
            //播放动画
            PlayAnim("");

            EHitForceType forceType = eve.GetParamByIndex<EHitForceType>(0);
            m_dir = eve.GetParamByIndex<Vector2>(1);
            m_speed = GetSpeed(forceType);

            if (m_rigidBody != null)
            {
                m_rigidBody.velocity = m_dir.normalized * m_speed;
            }

            if (m_particle != null && forceType >= EHitForceType.Middle)
            {
                m_particle.gameObject.SetActive(true);
            }
        }
        
    }

    private float GetSpeed(EHitForceType forceType)
    {
        float value = m_speedRate.y;
        switch (forceType)
        {
               case EHitForceType.Soft:
                   value = m_speedRate.x;
                   break;
               case EHitForceType.Middle:
                   value = m_speedRate.y;
                   break;
               case EHitForceType.High:
                   value = m_speedRate.z;
                   break;
        }

        return value;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                ContactPoint2D contactPoint = other.contacts[0];
                Vector3 newDir = Vector3.Reflect(m_dir, contactPoint.normal);
                newDir.z = 0f;
                //Quaternion rotation = Quaternion.FromToRotation(m_dir,  newDir);
                //transform.rotation = rotation;
                if (m_rigidBody != null)
                {
                    m_rigidBody.velocity = newDir.normalized * m_speed;
                }

                m_dir = newDir;
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
            {
                GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
                if (eventModuel != null)
                {
                    eventModuel.SendEvent(GameEventID.Reset_Game_State, true, 0f);
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameBallManager.GetInstance().UnRegisterBall(this);
    }
}
