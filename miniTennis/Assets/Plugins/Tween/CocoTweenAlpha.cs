using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CocoPlay
{
	public class CocoTweenAlpha : CocoTweenBase
	{
		[Header ("Tween Range")]
		public CocoFloatRange alphaRange = new CocoFloatRange (0, 1);


		#region implemented abstract members of CocoTweenBase

		protected override void TweenInit (bool reversed)
		{
			float alpha = Mathf.Clamp01 (reversed ? alphaRange.To : alphaRange.From);

			Image image = GetComponent<Image> ();
			if (image != null) {
				Color color = image.color;
				color.a = alpha;
				image.color = color;
				return;
			}

			Text text = GetComponent<Text> ();
			if (text != null) {
				Color color = text.color;
				color.a = alpha;
				text.color = color;
				return;
			}

			Renderer renderer = GetComponent<Renderer> ();
			if (renderer != null) {
				Color color = renderer.material.color;
				color.a = alpha;
				renderer.material.color = color;
			}
		}

		protected override LTDescr TweenRun (bool reversed, float time)
		{
			float alpha = Mathf.Clamp01 (reversed ? alphaRange.To : alphaRange.From);

			Image image = GetComponent<Image> ();
			if (image != null) {
				return LeanTween.alpha ((RectTransform)transform, alpha, time);
			}

			Text text = GetComponent<Text> ();
			if (text != null) {
				return LeanTween.textAlpha ((RectTransform)transform, alpha, time);
			}

			Renderer renderer = GetComponent<Renderer> ();
			if (renderer != null) {
				return LeanTween.alpha (gameObject, alpha, time);
			}

			return null;
		}

		#endregion
		
	}
}
