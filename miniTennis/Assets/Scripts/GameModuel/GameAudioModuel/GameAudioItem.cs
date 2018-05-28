using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioItem : MonoBehaviour
{
	public AudioSource m_souce;

	public AudioClip m_clip;

	public float m_time;
	private bool m_start;
	
	void Start ()
	{
	}

	public float PlayAudio(string name)
	{
		m_souce = gameObject.AddComponent<AudioSource>();
		GameResModuel resmoduel = GameStart.GetInstance().ResModuel;
		m_clip = resmoduel.LoadResources<AudioClip>(EResourceType.Audio, name);
		m_souce.clip = m_clip;
		m_souce.Play();
		m_time = 0;
		m_start = true;
		return m_clip == null ? 0f : m_clip.length;
	}
	
	void Update ()
	{
		if (m_start)
		{
			m_time += Time.deltaTime;
			if (m_clip == null || (m_clip != null && m_time > m_clip.length))
			{
				Destroy(gameObject);
			}
		}
	}
}
