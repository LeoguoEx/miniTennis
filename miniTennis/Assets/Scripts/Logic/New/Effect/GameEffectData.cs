using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectData
{
    public Rect m_createEffectRect;

    private Vector2 m_effectCreateDuringTime;
    public float m_moveSpeed;

    public GameEffectData()
    {
        m_effectCreateDuringTime = new Vector2(2f, 5f);
        m_createEffectRect = new Rect(-3.71f, 3.71f, 0f, 0f);
        m_moveSpeed = 1f;
    }

    public float GetRandomTime()
    {
        return Random.Range(m_effectCreateDuringTime.x, m_effectCreateDuringTime.y);
    }
}
