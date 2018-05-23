using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CocoPlay
{
	[ExecuteInEditMode]
	public class CocoUIAnchor : MonoBehaviour
	{
		public enum AnchorPos
		{
			TopLeft = 1,
			Top = 2,
			TopRight = 3,

			MiddleLeft = 4,
			Middle = 5,
			MiddleRight = 6,

			BottomLeft = 7,
			Bottom = 8,
			BottomRight = 9,
		}

		[SerializeField]
		public AnchorPos anchorPos = AnchorPos.Middle;
		CocoDetectableValue<AnchorPos> m_DetectableAnchorPos = null;

		public CocoUIAnchor.AnchorPos UIAnchorPos {
			get {
				return m_DetectableAnchorPos.Value;
			}
			set {
				m_DetectableAnchorPos.Value = value;
			}
		}

		void Update ()
		{
			if (m_DetectableAnchorPos == null) {
				UpdateAnchor (anchorPos);

				m_DetectableAnchorPos = new CocoDetectableValue<AnchorPos> (anchorPos);
				m_DetectableAnchorPos.onValueChanged = UpdateAnchor;
			}

			m_DetectableAnchorPos.Value = anchorPos;
		}

		void UpdateAnchor (AnchorPos anchorPos)
		{
			RectTransform trans = GetComponent<RectTransform> ();
			if (trans == null) {
				return;
			}

			Vector2 pos = new Vector2 (0.5f, 0.5f);

			switch (anchorPos) {
			case AnchorPos.TopLeft:
			case AnchorPos.MiddleLeft:
			case AnchorPos.BottomLeft:
				pos.x = 0f;
				break;

			case AnchorPos.TopRight:
			case AnchorPos.MiddleRight:
			case AnchorPos.BottomRight:
				pos.x = 1f;
				break;
			}
			switch (anchorPos) {
			case AnchorPos.TopLeft:
			case AnchorPos.Top:
			case AnchorPos.TopRight:
				pos.y = 1f;
				break;

			case AnchorPos.BottomLeft:
			case AnchorPos.Bottom:
			case AnchorPos.BottomRight:
				pos.y = 0f;
				break;
			}

			trans.anchorMin = pos;
			trans.anchorMax = pos;
			trans.sizeDelta = Vector2.zero;
			trans.name = "Anchor_" + anchorPos;
		}
	}

}

