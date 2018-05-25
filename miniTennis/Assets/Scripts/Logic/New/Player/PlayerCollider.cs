using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider
{
    private Rect m_moveArea;        //角色移动范围
    private float m_areaRadius;     //角色击球扇形半径
    private float m_areaAngle;      //角色击球扇形角度

    private BoxCollider2D m_boxCollider2D;

    public BoxCollider2D BoxCollider2D
    {
        get { return m_boxCollider2D; }
    }
    
    public PlayerCollider(Rect area, float radius, float angle, BoxCollider2D collider)
    {
        m_moveArea = area;
        m_areaRadius = radius;
        m_areaAngle = angle;
        m_boxCollider2D = collider;
    }

    public static bool CheckInHitBallArea(Transform target, Transform player, float radius, float angle, BoxCollider2D collider)
    {
        //与敌人的距离
        float distance = Vector3.Distance(player.position, target.position);
        //玩家正前方的向量
        Vector3 norVec = player.rotation * Vector3.up;
        //玩家与敌人的方向向量
        Vector3 temVec = target.position - player.position;
        //求两个向量的夹角
        float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;
        if (distance < radius)
        {
            if (jiajiao <= angle * 0.5f)
            {
                Debug.Log("在扇形范围内");
                return true;
            }
        }

        if (collider != null)
        {
            Rect rect = new Rect(player.position.x - collider.size.x * 0.5f, player.position.x + collider.size.x * 0.5f,
                player.position.y - collider.size.y * 0.5f, player.position.y + collider.size.y * 0.5f);
            if (rect.x < target.position.x && target.position.x < rect.y && rect.width < target.position.y &&
                target.position.y < rect.height)
            {
                return true;
            }
        }
        
        return false;
    }

    public Vector2 AdjustMoveArea(Vector3 movePosition)
    {
        return movePosition;
    }

    public Vector2 GetHitBallDirection(float angle, Vector2 forward)
    {
        Vector2 direction = Quaternion.Euler(new Vector3(0f, 0f, angle)) * forward;
        return direction.normalized;
    }
}
