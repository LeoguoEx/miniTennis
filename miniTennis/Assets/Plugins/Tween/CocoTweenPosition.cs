using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public class CocoTweenPosition : CocoTweenBase
	{
		[Header ("Tween Range")]
		public CocoVector3Range positionRange = new CocoVector3Range (Vector3.zero, Vector3.one);
		public bool isLocal = true;

		#region implemented abstract members of CocoTweenBase

		protected override void TweenInit (bool reversed)
		{
			Vector3 pos = reversed ? positionRange.To : positionRange.From;
			if (isLocal) {
				transform.localPosition = pos;
			} else {
				transform.position = pos;
			}
		}

		protected override LTDescr TweenRun (bool reversed, float time)
		{
			Vector3 pos = reversed ? positionRange.To : positionRange.From;
			if (isLocal) {
				return LeanTween.moveLocal (gameObject, pos, time);
			}
			return LeanTween.move (gameObject, pos, time);
		}

		#endregion
		
	}
}
