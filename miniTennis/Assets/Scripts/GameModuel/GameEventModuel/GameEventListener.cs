using System.Collections.Generic;

public delegate void DGameEventListener(GameEvent gameEvent);

public class GameEventListener
{
    private List<DGameEventListener> m_listeners;

    public GameEventListener()
    {
        m_listeners = new List<DGameEventListener>();
    }

    //注册事件
    public void RegisterEventListener(DGameEventListener listener)
    {
        if (!m_listeners.Contains(listener))
        {
            m_listeners.Add(listener);
        }
    }

    //取消注册事件
    public void UnRegisterEventListener(DGameEventListener listener)
    {
        if (m_listeners.Contains(listener))
        {
            m_listeners.Remove(listener);
        }
    }

    //清空事件
    public void DisposeListener()
    {
        if (m_listeners.Count > 0)
        {
            m_listeners.Clear();
        }
    }

    //发送事件
    public void DispatchGameEvent(GameEvent gameEvent)
    {
        if (gameEvent == null)
        {
            return;
        }

        for (int i = 0; i < m_listeners.Count; i++)
        {
            DGameEventListener listener = m_listeners[i];
            if (listener == null)
            {
                continue;
            }

            listener(gameEvent);
        }
    }
}
