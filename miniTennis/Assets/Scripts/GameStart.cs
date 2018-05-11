using UnityEngine;

public class GameStart : MonoBehaviour
{
    private static GameStart m_instance;

    public static GameStart GetInstance()
    {
        if (m_instance == null)
        {
            GameObject gameObj = new GameObject("GameStart");
            gameObj.SetTransformZero();
            DontDestroyOnLoad(gameObj);
            m_instance = CommonFunc.AddSingleComponent<GameStart>(gameObj);
        }
        return m_instance;
    }

    #region Event

    private GameEventModuel m_eventModuel;
    public GameEventModuel EventModuel
    {
        get
        {
            if (m_eventModuel == null)
            {
                m_eventModuel = gameObject.AddComponent<GameEventModuel>();
            }
            return m_eventModuel;
        }
    }

    #endregion

    #region Log

    private GameLogModuel m_logModuel;

    public GameLogModuel LogModuel
    {
        get
        {
            if (m_logModuel == null)
            {
                m_logModuel = gameObject.AddComponent<GameLogModuel>();
            }

            return m_logModuel;
        }
    }

    #endregion

    #region Resources

    private GameResModuel m_resModuel;

    public GameResModuel ResModuel
    {
        get
        {
            if (m_resModuel == null)
            {
                m_resModuel = gameObject.AddComponent<GameResModuel>();
            }

            return m_resModuel;
        }
    }

    #endregion

    #region State

    private GameStateModuel m_stateModuel;

    public GameStateModuel StateModuel
    {
        get
        {
            if (m_stateModuel == null)
            {
                m_stateModuel = gameObject.AddComponent<GameStateModuel>();
            }

            return m_stateModuel;
        }
    }

    #endregion

    #region Data

    private GameDataModuel m_dataModuel;

    public GameDataModuel DataModuel
    {
        get
        {
            if (m_dataModuel == null)
            {
                m_dataModuel = gameObject.AddComponent<GameDataModuel>();
            }

            return m_dataModuel;
        }
    }

    #endregion

    void Start()
    {
        m_instance = this;
        InitGameModuel();
    }

    private void InitGameModuel()
    {
        LogModuel.Init();
        EventModuel.Init();
        ResModuel.Init();
        StateModuel.Init();
    }
}
