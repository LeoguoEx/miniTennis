using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CocoPlay
{
	public class CocoTweenColor : CocoTweenBase
	{
		[Header ("Tween Range")]
		public CocoColorRange colorRange = new CocoColorRange (Color.clear, Color.white);


		#region implemented abstract members of CocoTweenBase

		protected override void TweenInit (bool reversed)
		{
			Color color = reversed ? colorRange.To : colorRange.From;

			Image image = GetComponent<Image> ();
			if (image != null) {
				image.color = color;
				return;
			}

			Text text = GetComponent<Text> ();
			if (text != null) {
				text.color = color;
				return;
			}

			Renderer renderer = GetComponent<Renderer> ();
			if (renderer != null) {
				renderer.material.color = color;
			}
		}

		protected override LTDescr TweenRun (bool reversed, float time)
		{
			Color color = reversed ? colorRange.To : colorRange.From;

			Image image = GetComponent<Image> ();
			if (image != null) {
				return LeanTween.color ((RectTransform)transform, color, time);
			}

			Text text = GetComponent<Text> ();
			if (text != null) {
				return LeanTween.textColor ((RectTransform)transform, color, time);
			}

			Renderer renderer = GetComponent<Renderer> ();
			if (renderer != null) {
				return LeanTween.color (gameObject, color, time);
			}

			return null;
		}

		#endregion
		
	}
}
