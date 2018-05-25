using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectData
{
    public Rect m_createEffectRect;

    private Vector2 m_effectCreateDuringTime;

    public GameEffectData()
    {
        m_effectCreateDuringTime = new Vector2(2f, 5f);
    }

    public float GetRandomTime()
    {
        return Random.Range(m_effectCreateDuringTime.x, m_effectCreateDuringTime.y);
    }
}
