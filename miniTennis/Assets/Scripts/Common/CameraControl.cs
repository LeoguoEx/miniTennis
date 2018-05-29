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

	public float showMaskTime = 0.01f;
	public float maskCurrentTime = 0.0f;
	public float maskTotalTime = 0.0f;

	private Vector3 m_recordPosition;

	private GameObject m_mask;

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

		m_mask = CommonFunc.GetChild(gameObject, "Mask");
		m_mask.SetActive(false);
	}
  
	public void Trigger()  
	{  
		totalTime = shakeTime;  
		currentTime = shakeTime;
	}

	public void TriggerMask()
	{
		maskTotalTime = showMaskTime;
		maskCurrentTime = showMaskTime;
	}
  
	public void Stop()  
	{  
		currentTime = 0.0f;  
		totalTime = 0.0f;  
	}

	public void UpdateMask()
	{
		if (maskCurrentTime > 0.0f && maskTotalTime > 0.0f)  
		{  
			float percent = maskCurrentTime / maskTotalTime;  
  
			maskCurrentTime -= Time.deltaTime;

			if (m_mask != null && !m_mask.activeSelf)
			{
				m_mask.SetActive(true);
			}
		}  
		else  
		{  
			maskCurrentTime = 0.0f;  
			maskTotalTime = 0.0f;
			if (m_mask != null && m_mask.activeSelf)
			{
				m_mask.SetActive(false);
			}
		}  
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
		UpdateMask();
	}  
  
	void OnEnable()  
	{  
		Trigger();  
	}  
}
