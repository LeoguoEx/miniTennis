using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private Camera m_camera;
	
	/// <summary>  
	/// 相机震动方向  
	/// </summary>  
	public Vector3 shakeDir = new Vector3(0.1f, 0.1f, 0f);
	/// <summary>  
	/// 相机震动时间  
	/// </summary>  
	public float shakeTime = 0.1f;    
  
	private float currentTime = 0.0f;  
	private float totalTime = 0.0f;

	private Vector3 m_recordPosition;

	private static CameraControl m_instance;

	public static CameraControl GetInstance()
	{
		if (m_instance == null)
		{
			GameObject go = GameObject.Find("Main Camera");
			m_instance = go.AddComponent<CameraControl>();
		}

		return m_instance;
	}
	
	void Start ()
	{
		m_camera = gameObject.GetComponent<Camera>();
		m_recordPosition = gameObject.transform.position;
	}
  
	public void Trigger()  
	{  
		totalTime = shakeTime;  
		currentTime = shakeTime;
	}  
  
	public void Stop()  
	{  
		currentTime = 0.0f;  
		totalTime = 0.0f;  
	}  
  
	public void UpdateShake()  
	{  
		if (currentTime > 0.0f && totalTime > 0.0f)  
		{  
			float percent = currentTime / totalTime;  
  
			Vector3 shakePos = Vector3.zero;  
			shakePos.x = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.x) * percent, Mathf.Abs(shakeDir.x) * percent);  
			shakePos.y = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.y) * percent, Mathf.Abs(shakeDir.y) * percent);  
			//shakePos.z = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.z) * percent, Mathf.Abs(shakeDir.z) * percent);  
  
			Camera.main.transform.position += shakePos;  
  
			currentTime -= Time.deltaTime;  
		}  
		else  
		{  
			currentTime = 0.0f;  
			totalTime = 0.0f;
			Camera.main.transform.position = m_recordPosition;
		}  
	}  
  
	void LateUpdate()  
	{  
		UpdateShake();  
	}  
  
	void OnEnable()  
	{  
		Trigger();  
	}  
}
