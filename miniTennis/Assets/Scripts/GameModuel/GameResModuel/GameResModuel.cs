using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EResourceType
{
	Scene = 0,
	Role = 1,
	UI = 2,
	Ball = 3,
	AnimController = 4,
	Effect = 5,
	Ground = 6,
    BallMechine = 7,
}

public delegate void DLoadResourcesCompete(UnityEngine.Object res);

public class ResLoadAsyncData
{
	public string m_path;
	public DLoadResourcesCompete m_action;
}

public class GameResModuel : GameModuelBase
{

	private List<ResLoadAsyncData> m_resPathList;
	private List<ResLoadAsyncData> m_cacheResPathList;
	private bool m_asyncEnd;
	
	protected void Awake()
	{
		
	}

	protected void Start()
	{
		
	}

	public override void Init()
	{
		Log(ELogType.Normal, "GameResModuel Start!!!!");
		m_resPathList = new List<ResLoadAsyncData>();
		m_cacheResPathList = new List<ResLoadAsyncData>();
		m_asyncEnd = true;
	}

	protected void OnDestory()
	{
	}

	public T LoadResources<T>(EResourceType resType, string name)
		where T : Object
	{
		string path = GameResFunc.GetResPath(resType, name);
		if (string.IsNullOrEmpty(path))
		{
			Log(ELogType.Error, string.Format("res path is null !!!!!      resType : {0}", resType));
			return null;
		}

		T res = Resources.Load<T>(path);
		return res;
	}

	public void LoadResourcesAsync<T>(EResourceType resType, string name, DLoadResourcesCompete action)
		where T : Object
	{
		string path = GameResFunc.GetResPath(resType, name);
		if (string.IsNullOrEmpty(path))
		{
			Log(ELogType.Error, string.Format("res path is null !!!!!      resType : {0}", resType));
			return;
		}

		ResLoadAsyncData data = GetAsyncData(path, action);
		m_resPathList.Add(data);
		if (m_asyncEnd)
		{
			StartCoroutine(LoadResAsync<T>());
		}
	}

	public void ResourcesUnLoad()
	{
		Resources.UnloadUnusedAssets();
	}

	private IEnumerator LoadResAsync<T>()
		where T : Object
	{
		while (true)
		{
			ResLoadAsyncData data = GetNeedLoadResData();
			if (data != null)
			{
				ResourceRequest request = Resources.LoadAsync<T>(data.m_path);

				while (!request.isDone)
				{
					yield return new WaitForEndOfFrame();
				}

				if (data.m_action != null)
				{
					data.m_action(request.asset as T);
				}
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}

	private ResLoadAsyncData GetNeedLoadResData()
	{
		if (m_resPathList.Count > 0)
		{
			ResLoadAsyncData data = m_resPathList[0];
			m_resPathList.RemoveAt(0);
			return data;
		}

		return null;
	}

	private ResLoadAsyncData GetAsyncData(string path, DLoadResourcesCompete action)
	{
		ResLoadAsyncData data = null;
		if (m_cacheResPathList.Count > 0)
		{
			data = m_cacheResPathList[0];
			m_cacheResPathList.RemoveAt(0);
		}
		else
		{
			data = new ResLoadAsyncData();
		}
		data.m_path = path;
		data.m_action = action;
		return data;
	}
}
