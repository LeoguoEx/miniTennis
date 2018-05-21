using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	private GroundData m_groundData;

	private GameObject m_leftPoint;
	private GameObject m_rightPoint;
	
	void Start ()
	{
	}

	public void InitGround(GroundData ground)
	{
		m_groundData = ground;
		m_leftPoint = CommonFunc.GetChild(gameObject, "LeftPoint");
		m_rightPoint = CommonFunc.GetChild(gameObject, "RightPoint");
	}

	public Vector3 GetLeftPoint()
	{
		if (m_leftPoint != null)
		{
			return m_leftPoint.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetRightPoint()
	{
		if (m_rightPoint != null)
		{
			return m_rightPoint.transform.position;
		}
		return Vector3.zero;
	}
}
