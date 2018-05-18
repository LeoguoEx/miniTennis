using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider
{
    private Rect m_moveArea;        //角色移动范围
    private float m_areaRadius;     //角色击球扇形半径
    private float m_areaAngle;      //角色击球扇形角度
    
    public PlayerCollider(Rect area, float radius, float angle)
    {
        m_moveArea = area;
        m_areaRadius = radius;
        m_areaAngle = angle;
    }

    public bool CheckInHitBallArea(Transform target, Transform player)
    {
        //与敌人的距离
        float distance = Vector3.Distance(player.position, target.position);
        //玩家正前方的向量
        Vector3 norVec = player.rotation * Vector3.forward;
        //玩家与敌人的方向向量
        Vector3 temVec = target.position - player.position;
        //求两个向量的夹角
        float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;
        if (distance < m_areaRadius)
        {
            if (jiajiao <= m_areaAngle * 0.5f)
            {
                Debug.Log("在扇形范围内");
            }
        }
        return false;
    }

    public Vector2 AdjustMoveArea(Vector3 movePosition)
    {
        return movePosition;
    }

    public Vector2 GetHitBallDirection(Vector2 moveDirection, Vector2 playerToBallDirection)
    {
        Vector2 direction = moveDirection + playerToBallDirection;
        return direction.normalized;
    }
}
