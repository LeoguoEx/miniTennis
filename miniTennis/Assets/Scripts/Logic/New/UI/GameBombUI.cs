using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBombUI : GameUIBase
{
    private Button m_button;
    private Text m_playerLastTime;
    private Text m_aiLastTime;

    public override void OnInit()
    {
        m_button = CommonFunc.GetChild(gameObject, "Button").GetComponent<Button>();
        m_playerLastTime = CommonFunc.GetChild(gameObject, "PlayerLastTime").GetComponent<Text>();
        m_aiLastTime = CommonFunc.GetChild(gameObject, "AILastTime").GetComponent<Text>();
    }

    public override void OnDispatch()
    {
    }

    public void SetPlayerLastTime(float time)
    {
        if (time < 0)
        {
            time = 0.0f;
        }
        if (m_playerLastTime != null)
        {
            m_playerLastTime.text = time.ToString("N1");
        }
    }

    public void SetAiLastTime(float time)
    {
        if (time < 0)
        {
            time = 0.0f;
        }
        if (m_aiLastTime != null)
        {
            m_aiLastTime.text = time.ToString("N1");
        }
    }
}
