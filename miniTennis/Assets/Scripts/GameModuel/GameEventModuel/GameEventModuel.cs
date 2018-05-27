using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventModuel : GameModuelBase
{
    private Dictionary<int, GameEventListener> m_gameEventListeners;

    private List<GameEvent> m_events;

    private List<GameEvent> m_cacheEvents; 

    protected void Awake()
    {
    }

    protected void Start()
    {
        
    }

    public override void Init()
    {
        Log(ELogType.Normal, "GameEventModuel Start!!!!");
        
        m_gameEventListeners = new Dictionary<int, GameEventListener>();
        m_events = new List<GameEvent>();
        m_cacheEvents = new List<GameEvent>();
    }

    protected void OnDestory()
    {
    }

    public void RegisterEventListener(int gameEventID, DGameEventListener action)
    {
        GameEventListener listener = null;
        if (!m_gameEventListeners.TryGetValue(gameEventID, out listener))
        {
            listener = new GameEventListener();
            m_gameEventListeners.Add(gameEventID, listener);
        }

        if (listener == null)
        {
            listener = new GameEventListener();
        }

        listener.RegisterEventListener(action);
    }

    public void UnRegisterEventListener(int gameEventID, DGameEventListener action)
    {
        GameEventListener listener = null;
        if (m_gameEventListeners.TryGetValue(gameEventID, out listener))
        {
            listener.UnRegisterEventListener(action);
        }
    }

    public void SendEvent(int gameEventID, bool immediately, float delayTime, params object[] args)
    {
        GameEvent gameEvent = GetEvent();
        gameEvent.InitGameEvent(gameEventID, immediately, delayTime, args);
        m_events.Insert(0, gameEvent);
    }

    protected void Update()
    {
        if (m_events == null || m_events.Count == 0)
        {
            return;
        }
        for (int i = m_events.Count - 1; i >= 0; i--)
        {
            GameEvent gameEvent = m_events[i];
            m_events.Remove(gameEvent);
            if (gameEvent.NeedSendEvent())
            {
                GameEventListener listener = GetGameEventListenerByEventID(gameEvent.EventID);
                if (listener != null)
                {
                    listener.DispatchGameEvent(gameEvent);
                }
            }
        }
    }

    private GameEvent GetEvent()
    {
        GameEvent gameEvent = null;
        if (m_cacheEvents == null || m_cacheEvents.Count == 0)
        {
            gameEvent = new GameEvent();
        }
        else
        {
            gameEvent = m_cacheEvents[0];
            m_cacheEvents.RemoveAt(0);
        }
        return gameEvent;
    }

    private GameEventListener GetGameEventListenerByEventID(int eventID)
    {
        GameEventListener listener = null;
        m_gameEventListeners.TryGetValue(eventID, out listener);
        return listener;
    }
}
