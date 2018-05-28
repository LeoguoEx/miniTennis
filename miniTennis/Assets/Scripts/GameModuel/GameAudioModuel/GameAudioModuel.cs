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
    private List<AudioSource> m_souceList;
    private List<AudioSource> m_cacheSourceList;

    private Dictionary<string, AudioClip> m_audioClipDic;
	
	public override void Init()
	{
	    m_souceList = new List<AudioSource>();
        m_cacheSourceList = new List<AudioSource>();
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

    void Update()
    {
        if (m_souceList.Count > 0)
        {
            for (int i = m_souceList.Count; i < m_souceList.Count; i--)
            {
                if (!m_souceList[i].isPlaying)
                {
                    m_cacheSourceList.Add(m_souceList[i]);
                    m_souceList.RemoveAt(i);
                }
            }
        }
    }

	public void PlayAudio(string name)
	{
	    AudioClip clip = null;
	    if (!m_audioClipDic.TryGetValue(name, out clip))
	    {
	        GameResModuel resModuel = GameStart.GetInstance().ResModuel;
	        clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, name);
        }
	    AudioSource source = GetAudioSource();
	    source.clip = clip;
	    source.Play();
	    m_souceList.Add(source);
	}

	public void playAudio(AudioClip clip)
	{
		AudioSource source = GetAudioSource();
		source.clip = clip;
		source.Play();
		m_souceList.Add(source);
	}

	public void PlayAudio(List<string> names)
	{
		StartCoroutine(PlayAudioSync(names));
	}

    private AudioSource GetAudioSource()
    {
        AudioSource souce = null;
        if (m_cacheSourceList.Count > 0)
        {
            souce = m_cacheSourceList[0];
            m_cacheSourceList.RemoveAt(0);
        }
        else
        {
            souce = m_go.AddComponent<AudioSource>();
        }
        return souce;
    }

	public void PlayBgAudio(string name)
	{
        StopAllCoroutines();

		if(name == m_bgName){return;}
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
        if(m_bgSource.clip == clip) { return; }
        m_bgSource.clip = clip;
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
			GameResModuel resModuel = GameStart.GetInstance().ResModuel;
			AudioClip clip = resModuel.LoadResources<AudioClip>(EResourceType.Audio, list[i]);

			playAudio(clip);

			yield return new WaitForSeconds(clip.length);
		}
	}
}
