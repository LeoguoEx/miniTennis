using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioModuel : GameModuelBase
{
	private AudioListener m_audioListener;
	private AudioSource m_bgSource;
	//private AudioSource m_clipSource;
	private string m_bgName;

    private GameObject m_go;

    private Dictionary<string, AudioClip> m_audioClipDic;
	
	public override void Init()
	{
        m_audioClipDic = new Dictionary<string, AudioClip>();
	    m_go = new GameObject("AudioListener");
		m_audioListener = m_go.AddComponent<AudioListener>();
		m_bgSource = m_go.AddComponent<AudioSource>();
		m_bgSource.loop = true;

		//m_clipSource = go.AddComponent<AudioSource>();
	}

    public void PreLoadAudio(List<string> list)
    {
        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
        for (int i = 0; i < list.Count; i++)
        {
            if (!m_audioClipDic.ContainsKey(list[i]))
            {
                resModuel.LoadResourcesAsync<AudioClip>(EResourceType.Audio, list[i], res =>
                {
                    m_audioClipDic.Add(res.name, res as AudioClip);
                });
            }
            
        }
    }

	public void PlayAudio(string name)
	{
	    GameObject go = new GameObject();
	    GameAudioItem item = go.AddComponent<GameAudioItem>();
	    item.PlayAudio(name);
	}

	public void PlayAudio(List<string> names)
	{
		StartCoroutine(PlayAudioSync(names));
	}

	public void PlayBgAudio(string name)
	{
        StopAllCoroutines();
        
		m_bgName = name;
	    AudioClip clip = null;
	    if (!m_audioClipDic.TryGetValue(name, out clip))
	    {
	        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
	        clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, name);
        }
	    PlayBgAudio(clip);

	}

    public void StopAudio()
    {
        if (m_bgSource != null)
        {
            m_bgSource.Stop();
        }
    }

    private void PlayBgAudio(AudioClip clip)
    {
	    m_bgSource.clip = clip;
        m_bgSource.enabled = false;
        m_bgSource.enabled = true;
        m_bgSource.Play();
    }

    public void PlayBgAudio(List<string> name)
    {
        StartCoroutine(PlayBgAudioSync(name));
    }

    private IEnumerator PlayBgAudioSync(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            m_bgName = name;
            AudioClip clip = null;
            if (!m_audioClipDic.TryGetValue(list[i], out clip))
            {
                GameResModuel resModuel = GameStart.GetInstance().ResModuel;
                clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, name);
            }

            PlayBgAudio(clip);

            yield return new WaitForSeconds(clip.length);
        }
    }
	
	private IEnumerator PlayAudioSync(List<string> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
		    GameObject go = new GameObject();
		    GameAudioItem item = go.AddComponent<GameAudioItem>();
		    float length = item.PlayAudio(list[i]);

            yield return new WaitForSeconds(length);
		}
	}
}
