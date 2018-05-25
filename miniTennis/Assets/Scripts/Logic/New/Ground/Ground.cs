using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	private GroundData m_groundData;

	private GameObject m_leftPoint;
	private GameObject m_rightPoint;

	private GameObject m_effectAI;
	private GameObject m_effectPlayer;
	
	private bool m_start;
	private float m_endTime;
	
	public GroundData GroundData
	{
		get { return m_groundData; }
	}
	
	void Start ()
	{
	}

	public void InitGround(GroundData ground)
	{
		m_groundData = ground;
		m_leftPoint = CommonFunc.GetChild(gameObject, "LeftPoint");
		m_rightPoint = CommonFunc.GetChild(gameObject, "RightPoint");
		m_effectAI = CommonFunc.GetChild(gameObject, "AIEffect");
		m_effectPlayer = CommonFunc.GetChild(gameObject, "PlayerEffect");

		int count = m_effectPlayer.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			Transform trans = m_effectPlayer.transform.GetChild(i);
			trans.gameObject.AddComponent<DetectEffect>();
		}
		
		HideEffect();
	}

	public Vector3 GetLeftPoint()
	{
		if (m_leftPoint != null)
		{
			return m_leftPoint.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetRightPoint()
	{
		if (m_rightPoint != null)
		{
			return m_rightPoint.transform.position;
		}
		return Vector3.zero;
	}

	public void ExcuteEffect(EffectBase effect, ESide side)
	{
		if (effect != null && effect is EffectShield)
		{
			EffectShield effectShield = effect as EffectShield;
			m_start = true;
			m_endTime = effectShield.m_duringTime + Time.time;
			GameObject go = null;
			if (side == ESide.AI)
			{
				go = m_effectAI;
			}
			else if (side == ESide.Player)
			{
				go = m_effectPlayer;
			}

			if (go != null)
			{
				go.SetActive(true);
			}
		}
	}

	private void Update()
	{
		if (m_start && Time.time > m_endTime)
		{
			m_start = false;
			HideEffect();
			GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
			eveModuel.SendEvent(GameEventID.END_GAME_EVENT, true, 0f);
		}
	}

	private void HideEffect()
	{
		if (m_effectAI != null)
		{
			m_effectAI.gameObject.SetActive(false);
		}

		if (m_effectPlayer != null)
		{
			m_effectPlayer.gameObject.SetActive(false);
		}
	}
}
