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
    private Vector3 m_speedRate = Vector3.one;
    
    
    [SerializeField]
    private Animator m_animator;


    private Vector3 m_moveDirection;
    
    private Vector3 m_rotationForce;

    private Rigidbody2D m_rigidBody;
    
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.RegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleHitBallMessage);
    }

    private void OnDisable()
    {
        GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
        eventModuel.UnRegisterEventListener(GameEventID.ENTITY_HIT_BALL, HandleHitBallMessage);
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

    private Vector3 Pong(Vector3 normal)
    {
        Vector3 pong = normal * 2 + m_moveDirection;
        
        //暂时把外力设置为0
        m_rotationForce = Vector3.zero;
        
        return pong;
    }

    private void HandleHitBallMessage(GameEvent eve)
    {
        if (eve != null)
        {
            //播放动画
            PlayAnim("");

            EHitForceType forceType = eve.GetParamByIndex<EHitForceType>(0);
            Vector2 direction = eve.GetParamByIndex<Vector2>(1);

            if (m_rigidBody != null)
            {
                Vector2 value = GetForceValue(forceType, direction);
                m_rigidBody.velocity = Vector2.zero;
                m_rigidBody.AddForce(value);
            }
        }
        
    }

    private Vector2 GetForceValue(EHitForceType forceType, Vector2 direction)
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

        return direction * value;
    }
}
