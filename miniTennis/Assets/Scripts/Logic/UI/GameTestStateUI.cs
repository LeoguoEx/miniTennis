using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTestStateUI : MonoBehaviour
{
	
    private GameObject[] m_hearts;
	[SerializeField]
	private GameObject m_heartParent;
	[SerializeField]
    private Text m_num;
    
	void Start ()
    {
	    
    }

	public void Init()
	{
		if (m_heartParent != null)
		{
			int childCount = m_heartParent.transform.childCount;
			m_hearts = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				m_hearts[i] = m_heartParent.transform.GetChild(i).gameObject;
			}
		}

		GameEventModuel eventModuel = GameStart.GetInstance().EventModuel;
		eventModuel.RegisterEventListener(GameEventID.GAME_DATA_CHANGE, OnDataChange);

		FreshUI();
	}

	private void FreshUI()
	{
		GameDataModuel dataModuel = GameStart.GetInstance().DataModuel;
		FreshHeart(dataModuel.m_heart);
		FreshNum(dataModuel.m_hitCount);
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
			m_num.text = num.ToString();
		}
	}

	private void OnDataChange(GameEvent eve)
	{
		FreshUI();
	}
}
