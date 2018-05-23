using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioModuel : GameModuelBase
{
	private AudioListener m_audioListener;
	private AudioSource m_bgSource;
	private AudioSource m_clipSource;
	private string m_bgName;
	
	public override void Init()
	{
		GameObject go = new GameObject("AudioListener");
		m_audioListener = go.AddComponent<AudioListener>();
		m_bgSource = go.AddComponent<AudioSource>();
		m_bgSource.loop = true;

		m_clipSource = go.AddComponent<AudioSource>();
	}

	public void PlayAudio(string name)
	{
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		AudioClip clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, name);
		m_clipSource.clip = clip;
		m_clipSource.Play();
	}

	public void PlayBgAudio(string name)
	{
		if(name == m_bgName){return;}
		m_bgName = name;
		GameResModuel resModuel = GameStart.GetInstance().ResModuel;
		AudioClip clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, name);
		m_bgSource.clip = clip;
		m_bgSource.Play();
	}
}
