using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELogType
{
	Normal = 0,
	Warning = 1,
	Error = 2,
}

public class GameLogModuel : GameModuelBase
{
	private List<string> m_logs;
	
	protected override void Awake()
	{
	}

	protected override void Start()
	{
		Log(ELogType.Normal, "Game Start!!!!");
		m_logs = new List<string>();
	}

	public override void Init()
	{
	}

	protected override void OnDestory()
	{
		WriteLogToFile();
		Log(ELogType.Normal, "Game End!!!!");
	}

	public void Log(ELogType logType, string log)
	{
		m_logs.Add(log);
		switch (logType)
		{
				case ELogType.Normal:
					Debug.Log(log);
					break;
				case ELogType.Warning:
					Debug.LogWarning(log);
					break;
				case ELogType.Error:
					Debug.LogError(log);
					break;
		}
	}

	private void WriteLogToFile()
	{
		
	}
}
