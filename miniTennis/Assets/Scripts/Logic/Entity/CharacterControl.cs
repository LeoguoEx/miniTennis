using System;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
	private GameObject m_entity;
	private bool m_enableMove;
	[SerializeField]
	private float m_moveSpeed = 0.1f;
	private Vector2 m_downPosition;
	private Vector3 m_recordDownPosition;
	
	private Vector4 m_moveRange = new Vector4(-2.28f, -1.05f, 2.34f, -4.35f);

	public Action<EEntityState, Vector3> SwitchState;

	private Vector3 m_recordPosition;

	void Start ()
	{
		FingerDownDetector downDetector = CommonFunc.AddSingleComponent<FingerDownDetector>(gameObject);
		FingerUpDetector upDetector = CommonFunc.AddSingleComponent<FingerUpDetector>(gameObject);
	}

	public void SetMoveEntity(GameObject go)
	{
		m_entity = go;
	}
	
	void Update () 
	{
		
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
	}


	void OnFingerDown( FingerDownEvent e )
	{
		m_enableMove = true;
		m_downPosition = e.Position;
		if (m_entity != null)
		{
			m_recordDownPosition = m_entity.transform.position;
			m_recordPosition = m_entity.transform.position;
		}

		if (SwitchState != null)
		{
			SwitchState(EEntityState.Prepare, Vector3.zero);
		}
	}
	
	void OnFingerMove( FingerMotionEvent e )
	{
		GameStart.GetInstance().LogModuel.Log(ELogType.Normal, "OnFingerMove    :  " + (e.Position * m_moveSpeed).ToString());
		Vector2 fingerDir = e.Position - m_downPosition;
		Vector3 entityMovePosition = m_recordDownPosition + new Vector3(fingerDir.x, fingerDir.y, 0f) * m_moveSpeed;
		m_entity.transform.position = CheckOutofRange(entityMovePosition);
	}

	void OnFingerStationary(FingerMotionEvent e)
	{
		if (m_entity != null)
		{
			m_recordPosition = m_entity.transform.position;
		}
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		m_enableMove = false;
		
		if (SwitchState != null)
		{
			SwitchState(EEntityState.Hit, m_recordPosition);
		}
	}

	private Vector3 CheckOutofRange(Vector3 point)
	{
		point.x = Mathf.Clamp(point.x, m_moveRange.x, m_moveRange.z);
		point.y = Mathf.Clamp(point.y, m_moveRange.w, m_moveRange.y);
		return point;
	}
}
