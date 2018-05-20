﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DHitBallDelegate(Vector2 direction, float force);

public class Player
{
    private PlayerAvatar m_avatar;
    private PlayerAnim m_anim;
    private PlayerCollider m_collider;
    private PlayerData m_playerData;

    private Vector2 m_playerMoveDirection;
    private DHitBallDelegate m_hitBallCallBack;

    public PlayerData PlayerData { get { return m_playerData; } }
    public Transform Transform { get { return m_avatar.transform; } }
    
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

            MovePosition(m_playerData.m_bornPosition);
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
        m_hitBallCallBack = null;
    }

    public void InitPlayerAction(DHitBallDelegate hitBallCall)
    {
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
        position.x = Mathf.Clamp(position.x, m_playerData.m_moveArea.x, m_playerData.m_moveArea.width);
        position.y = Mathf.Clamp(position.y, m_playerData.m_moveArea.height, m_playerData.m_moveArea.y);
        if (m_avatar != null)
        {
            m_avatar.Move(position);
        }
    }

    private void AdjustMoveRange(Vector2 position)
    {
        
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
        Vector3 dir = m_playerMoveDirection.normalized;
        float angle = m_playerData.GetFireBallAngle(dir.x);
        Vector2 direction = m_collider.GetHitBallDirection(angle, m_avatar.transform.rotation * Vector3.up);
        float force = m_playerData.GetFireBallForce(m_playerMoveDirection.y);
        if (m_hitBallCallBack != null)
        {
            m_hitBallCallBack(direction, force);
        }
    }
}
