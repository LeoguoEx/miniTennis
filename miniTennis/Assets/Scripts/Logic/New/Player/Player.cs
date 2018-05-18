using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate GameObject DGetCurPlayerTarget();

public delegate void DHitBallDelegate(Vector2 direction, float force);

public class Player
{
    private PlayerAvatar m_avatar;
    private PlayerAnim m_anim;
    private PlayerCollider m_collider;
    private PlayerData m_playerData;

    private Vector2 m_playerMoveDirection;

    private DGetCurPlayerTarget m_targetCall;
    private DHitBallDelegate m_hitBallCallBack;
    
    public Player(PlayerData playerData)
    {
        if(playerData == null){return;}
        m_playerData = playerData;
        
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        GameObject player = resModuel.LoadResources<GameObject>(EResourceType.Role, playerData.m_playerResPath);
        player = CommonFunc.Instantiate(player);
        if (player != null)
        {
            m_avatar = player.AddComponent<PlayerAvatar>();
            
            m_anim = new PlayerAnim(player);
            m_anim.InitAnimator(playerData.m_animControllerName);
            
            m_collider = new PlayerCollider(playerData.m_moveArea, playerData.m_radius, playerData.m_angle);
        }
    }

    public void Destroy()
    {
        if (m_avatar != null)
        {
            GameObject.Destroy(m_avatar.gameObject);
        }


        m_anim = null;
        m_collider = null;
        m_targetCall = null;
        m_hitBallCallBack = null;
    }

    public void InitPlayerAction(DGetCurPlayerTarget targetCall, DHitBallDelegate hitBallCall)
    {
        m_targetCall = targetCall;
        m_hitBallCallBack = hitBallCall;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        m_playerMoveDirection = direction;
    }

    public void StartMove()
    {
        if (m_anim != null)
        {
            m_anim.PlayAnim(EEntityState.Prepare);
        }
    }

    public void EndMove()
    {
        FireBall();
    }

    public void MovePosition(Vector2 position)
    {
        if (m_avatar != null)
        {
            m_avatar.Move(position);
        }
    }

    private void FireBall()
    {
        if (m_anim != null)
        {
            m_anim.PlayAnim(EEntityState.Hit);
        }
        
        //动画事件触发
        HitBall();
    }

    public float GetPlayerMoveSpeed()
    {
        if (m_playerData != null)
        {
            return m_playerData.m_moveSpeed;
        }

        return 0f;
    }

    public Vector3 GetPlayerPosition()
    {
        if (m_avatar != null)
        {
            return m_avatar.GetPosition();
        }
        return Vector3.zero;
    }

    private void HitBall()
    {
        if (m_targetCall != null && m_collider != null)
        {
            GameObject target = m_targetCall();
            if (target != null)
            {
                bool isInArea = m_collider.CheckInHitBallArea(target.transform, m_avatar.transform);
                if (isInArea)
                {
                    Vector2 playerToBallDir = target.transform.position - m_avatar.GetPosition();
                    Vector2 direction = m_collider.GetHitBallDirection(m_playerMoveDirection, playerToBallDir);
                    float force = m_playerData.GetFireBallForce(m_playerMoveDirection, playerToBallDir);
                    m_hitBallCallBack(direction, force);
                }
            }
        }
    }
}
