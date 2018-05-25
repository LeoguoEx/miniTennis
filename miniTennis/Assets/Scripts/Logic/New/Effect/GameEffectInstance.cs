using System.Collections;
using System.Collections.Generic;
using CocoPlay;
using UnityEngine;

public class GameEffectInstance : MonoBehaviour
{
	private bool m_trigger;
	
	void Start ()
	{
		m_trigger = false;
	}

	public void PlayTweenScale()
	{
		LeanTween.scale(gameObject, Vector3.one, 0.5f).setFrom(new Vector3(0.01f, 0.01f, 0.01f)).setTime(0.5f).setLoopOnce();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		m_trigger = true;
		GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
		eveModuel.SendEvent(GameEventID.TRIGGER_GAME_EVENT, true, 0f);
		gameObject.SetActive(false);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!m_trigger)
		{
			GameEventModuel eveModuel = GameStart.GetInstance().EventModuel;
			eveModuel.SendEvent(GameEventID.TRIGGER_GAME_EVENT, true, 0f);
			gameObject.SetActive(false);
			m_trigger = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		m_trigger = false;
	}
}
