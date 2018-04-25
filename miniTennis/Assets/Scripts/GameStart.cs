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
            m_instance = CommonFunc.AddSingleComponent<GameStart>(gameObj);
        }
        return m_instance;
    }

}
