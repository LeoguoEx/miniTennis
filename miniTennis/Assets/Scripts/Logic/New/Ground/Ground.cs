using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	private GroundData m_groundData;
	
	void Start () 
	{
		
	}

	public void InitGround(GroundData ground)
	{
		m_groundData = ground;
	}
}
