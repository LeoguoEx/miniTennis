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
	}

	public void AddHeart()
	{
		m_heart++;
	}

	public void AddHitCount()
	{
		m_hitCount++;
	}
}
