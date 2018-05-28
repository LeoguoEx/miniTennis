using System.Collections;
using System.Collections.Generic;
using CocoPlay;
using UnityEngine;
using UnityEngine.UI;

public class GameContestUI : GameUIBase
{
	private GameObject[] m_hearts;
	private GameObject m_heartParent;
	private Text m_num;

	private GameObject m_end;
	private Text m_endNum;
	
	public override void OnInit()
	{
		m_heartParent = CommonFunc.GetChild(gameObject, "Heart");
	    GameObject num = CommonFunc.GetChild(gameObject, "Num");
        m_num = num.GetComponent<Text>();

        if (m_heartParent != null)
		{
			int childCount = m_heartParent.transform.childCount;
			m_hearts = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				m_hearts[i] = m_heartParent.transform.GetChild(i).gameObject;
			}
		}

		m_end = CommonFunc.GetChild(gameObject, "End");
		m_endNum = CommonFunc.GetChild(gameObject, "EndNum").GetComponent<Text>();
		Button back = CommonFunc.GetChild(gameObject, "Back").GetComponent<Button>();
		back.onClick.AddListener(SwitchState);
		
		if (m_end != null)
		{
			m_end.SetActive(false);
		}
	}

	public void GameEnd()
	{
		if (m_end != null)
		{
			m_end.SetActive(true);
		}

		m_endNum.text = m_num.text;
		m_num.gameObject.SetActive(false);
	}

	public void FreshUI(int heartCount, int num)
	{
		FreshHeart(heartCount);
		FreshNum(num);
	}

	private void FreshHeart(int heartCount)
	{
		for (int i = 0; i < m_hearts.Length; i++)
		{
			if (m_hearts[i] != null)
			{
				m_hearts[i].SetActive(i < heartCount);
			}
		}
	}

	private void FreshNum(int num)
	{
		if (m_num != null)
		{
			m_num.gameObject.SetActive(true);
			m_num.text = num.ToString();

		    if (num != 0)
		    {
		        CocoTweenScale scale = m_num.GetComponent<CocoTweenScale>();
		        scale.TweenOnce(false, true);
            }
		}
	}

	public override void OnDispatch()
	{
		
	}

	private void SwitchState()
	{
		GameAudioModuel moduel = GameStart.GetInstance().AudioModuel;
		moduel.PlayAudio("click_01");
		GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
		eveModuel.SendEvent(GameEventID.SWITCH_GAME_STATE, true, 0f, EGameStateType.GameMenuState);
	}
}
