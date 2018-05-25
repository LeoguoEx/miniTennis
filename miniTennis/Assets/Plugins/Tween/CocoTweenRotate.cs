using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public class CocoTweenRotate : CocoTweenBase
	{
		[Header ("Tween Range")]
		public CocoVector3Range angleRange = new CocoVector3Range (Vector3.zero, Vector3.one);
		public bool isLocal = true;

		#region implemented abstract members of CocoTweenBase

		protected override void TweenInit (bool reversed)
		{
			Vector3 angle = reversed ? angleRange.To : angleRange.From;
			if (isLocal) {
				transform.localEulerAngles = angle;
			} else {
				transform.localEulerAngles = angle;
			}
		}

		protected override LTDescr TweenRun (bool reversed, float time)
		{
			Vector3 angle = reversed ? angleRange.To : angleRange.From;
			if (isLocal) {
				return LeanTween.rotateLocal (gameObject, angle, time);
			}
			return LeanTween.rotate (gameObject, angle, time);
		}

		#endregion
		
	}
}
