using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public class CocoTweenScale : CocoTweenBase
	{
		[Header ("Tween Range")]
		public CocoVector3Range scaleRange = new CocoVector3Range (Vector3.one, Vector3.one * 1.1f);

		#region implemented abstract members of CocoTweenBase

		protected override void TweenInit (bool reversed)
		{
			Vector3 scale = reversed ? scaleRange.To : scaleRange.From;
			transform.localScale = scale;
		}

		protected override LTDescr TweenRun (bool reversed, float time)
		{
			Vector3 scale = reversed ? scaleRange.To : scaleRange.From;
			return LeanTween.scale (gameObject, scale, time);
		}

		#endregion
		
	}
}
