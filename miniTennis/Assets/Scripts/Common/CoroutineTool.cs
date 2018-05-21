using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTool : MonoBehaviour
{
	private static CoroutineTool m_instance;
	public static CoroutineTool GetInstance()
	{
		if (m_instance == null)
		{
			GameObject go = new GameObject("CoroutineTool");
			m_instance = go.AddComponent<CoroutineTool>();
		}

		return m_instance;
	}
	
	void Start () 
	{
		
	}

	public void StartGameCoroutine(IEnumerator enumerator)
	{
		StartCoroutine(enumerator);
	}

	public void StopGameCoroutine(IEnumerator enumerator)
	{
		StopCoroutine(enumerator);
	}

	public void StopGameAllCoroutines()
	{
		StopAllCoroutines();
	}
}
