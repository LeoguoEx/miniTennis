using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CocoPlay
{
	public class CocoTweenShowOrHide : MonoBehaviour
	{
		void Awake ()
		{
			Init ();
		}

		void Start ()
		{
			if (showAutomatically) {
				Show ();
			}
		}

		#region Status

		enum Status
		{
			Hided,
			Appear,
			Showed,
			Disappear
		}

		Status m_Status = Status.Hided;
		public bool showedOnStart = false;
		public bool showAutomatically = false;
		bool m_IsInited = false;

		void Init ()
		{
			if (m_IsInited) {
				return;
			}

			int count = Tweens.Length;
			for (int i = 0; i < count; i++) {
				CocoTweenBase tween = Tweens [i];
				if (!tween.enabled) {
					continue;
				}

				tween.startAutomatically = false;
				tween.tweenTime = showTime;
				tween.tweenWrapMode = WrapMode.ClampForever;
				tween.Init (showedOnStart);
			}

			m_Status = showedOnStart ? Status.Showed : Status.Hided;

			m_IsInited = true;
		}

		#endregion


		#region Show/Hide

		public float showTime = 0.3f;

		public bool IsShowed {
			get {
				return m_Status == Status.Showed;
			}
		}

		public bool IsHided {
			get {
				return m_Status == Status.Hided;
			}
		}

		public void Show (System.Action endAction = null)
		{
			ShowOrHide (true, endAction);
		}

		public void Hide (System.Action endAction = null)
		{
			ShowOrHide (false, endAction);
		}

		void ShowOrHide (bool show, System.Action endAction)
		{
			Init ();

			Status targetStatus = show ? Status.Showed : Status.Hided;

			// already be target status
			if (m_Status == targetStatus) {
				if (endAction != null) {
					endAction ();
				}
				return;
			}

			// cancel last tween
			LeanTween.cancel (gameObject);

			// end action
			int usedFlags = 0;
			System.Action<int> tweenEndAction = (flag) => {
				usedFlags &= (~flag);
				if (usedFlags == 0) {
					m_Status = targetStatus;
					if (endAction != null) {
						endAction ();
					}
				}
			};

			// start tween
			m_Status = show ? Status.Appear : Status.Disappear;
			int count = Tweens.Length;
			if (count > 32) {
				count = 32;
			}

			for (int i = 0; i < count; i++) {
				CocoTweenBase tween = Tweens [i];
				if (!tween.enabled) {
					continue;
				}

				int flag = 1 << i;
				usedFlags |= flag;
				bool canRun = tween.TweenOnce (!show, false, () => {
					tweenEndAction (flag);
				});

				if (!canRun) {
					usedFlags &= ~flag;
				}
			}

			// all unused
			if (usedFlags == 0) {
				m_Status = targetStatus;
				if (endAction != null) {
					endAction ();
				}
			}
		}

		#endregion


		#region Tweens

		[SerializeField]
		CocoTweenBase[] m_Tweens = null;

		public CocoTweenBase[] Tweens {
			get {
				if (m_Tweens == null || m_Tweens.Length <= 0) {
					RefreshTweens ();
				}
				return m_Tweens;
			}
		}

		public void RefreshTweens ()
		{
			m_Tweens = GetComponents<CocoTweenBase> ();
		}

		#endregion

	}
}
