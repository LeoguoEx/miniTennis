using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : PlayerControllerBase
{

	public override void InitController(Player player)
	{
	}

	public override void DestroyController()
	{
		GameObject.Destroy(this);
	}
}
