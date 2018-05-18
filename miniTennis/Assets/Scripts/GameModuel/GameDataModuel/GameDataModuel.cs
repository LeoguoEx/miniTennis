using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataModuel : GameModuelBase
{
	public int m_heart;
	public int m_hitCount;
		
	public override void Init()
	{
		
	}

	public void SetGameData(int heart, int hitCount)
	{
		m_heart = heart;
		m_hitCount = hitCount;
	}

	public void ReduceHeart()
	{
		m_heart--;
		m_heart = Mathf.Clamp(m_heart, 0, 6);
		SendEvent();
	}

	public void AddHeart()
	{
		m_heart++;
		SendEvent();
	}

	public void AddHitCount()
	{
		m_hitCount++;
		SendEvent();
	}

	private void SendEvent()
	{
		GameEventModuel moduel = GameStart.GetInstance().EventModuel;
		moduel.SendEvent(GameEventID.GAME_DATA_CHANGE, true, 0f);
	}
}
