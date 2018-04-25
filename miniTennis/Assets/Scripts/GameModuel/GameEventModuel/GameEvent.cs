using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    /// <summary>
    /// 事件ID
    /// </summary>
    private int m_eventId;

    /// <summary>
    /// 是否立即发送
    /// </summary>
    private bool m_immediately;

    /// <summary>
    /// 延迟时间发送
    /// </summary>
    private float m_sendTime;

    /// <summary>
    /// 事件参数
    /// </summary>
    private object[] m_args;

    public int EventID { get { return m_eventId; } }

    public bool Immediately{get { return m_immediately; }}

    public GameEvent()
    {
    }

    public void InitGameEvent(int eventID, bool immediately, float delayTime, params object[] args)
    {
        m_eventId = eventID;
        m_immediately = immediately;
        m_sendTime = Time.time + delayTime;
        m_args = args;
    }

    public bool NeedSendEvent()
    {
        return (Time.time >= m_sendTime);
    }

    public T GetParamByIndex<T>(int index)
    {
        if (m_args == null || index < 0 || index > m_args.Length)
        {
            return default(T);
        }

        if (m_args[index] != null && !(m_args[index] is T))
        {
            return default(T);
        }

        try
        {
            return (T)m_args[index];
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        return default(T);
    }
}
