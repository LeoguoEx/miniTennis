using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBombUI : GameUIBase
{
    private Button m_button;
    private Text m_playerLastTime;
    private Text m_aiLastTime;

    private GameObject m_end;
    private Button m_back;
    private Text m_die;

    private bool m_win;
    
    public override void OnInit()
    {
        m_button = CommonFunc.GetChild(gameObject, "Button").GetComponent<Button>();
        m_playerLastTime = CommonFunc.GetChild(gameObject, "PlayerLastTime").GetComponent<Text>();
        m_aiLastTime = CommonFunc.GetChild(gameObject, "AILastTime").GetComponent<Text>();
        m_end = CommonFunc.GetChild(gameObject, "End");
        m_back = CommonFunc.GetChild(gameObject, "Back").GetComponent<Button>();
        m_die = CommonFunc.GetChild(gameObject, "Die").GetComponent<Text>();
        m_end.SetActive(false);
        
        m_back.onClick.AddListener(OnBackClick);
    }

    public override void OnDispatch()
    {
    }

    public void ShowEnd(bool win)
    {
        m_win = win;
        StartCoroutine(ShowEndCoroutine());
    }

    private IEnumerator ShowEndCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        if (m_end != null)
        {
            m_end.SetActive(true);
        }

        if (m_win)
        {
            m_die.text = "You Win!";
        }
        else
        {
            m_die.text = "You Died!";
        }
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

    private void OnBackClick()
    {
        GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
        moduel.PlayAudio("click_01");
        GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
        eveModuel.SendEvent(GameEventID.SWITCH_GAME_STATE, true, 0f, EGameStateType.GameMenuState);
    }
}
