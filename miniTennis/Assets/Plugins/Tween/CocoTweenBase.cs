using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public abstract class CocoTweenBase : MonoBehaviour
	{

		void Start ()
		{
			Init ();
		}

		void OnEnable ()
		{
			if (startAutomatically) {
				Tween ();
			}
		}

		void OnDisable ()
		{
			TweenStop ();
		}

		#region Init

		[SerializeField]
		bool m_StartByReversed = false;
		public bool startAutomatically = true;
		bool m_IsInited = false;
		public bool ignoreTimeScale = true;

		void Init ()
		{
			if (m_IsInited) {
				return;
			}

			TweenInit (m_StartByReversed);
			m_IsInited = true;
		}

		public void Init (bool reversed)
		{
			m_StartByReversed = reversed;
			TweenInit (m_StartByReversed);
			m_IsInited = true;
		}

		protected abstract void TweenInit (bool reversed);

		#endregion


		#region Tween Property

		[Header ("Tween Property")]
		public float tweenTime = 0.3f;
		public LeanTweenType tweenEaseType = LeanTweenType.easeInOutQuad;
		public WrapMode tweenWrapMode = WrapMode.PingPong;
		[SerializeField]
		int m_TweenLoopCount = -1;

		int m_LastTweenId = -1;

		#endregion

		#region Tween

		public bool TweenLoop (bool reversed, int loopCount, bool pingPong = true, System.Action endAction = null)
		{
			m_StartByReversed = reversed;
			m_TweenLoopCount = loopCount;
			tweenWrapMode = pingPong ? WrapMode.PingPong : WrapMode.Loop;
			return Tween (endAction);
		}

		public bool TweenOnce (bool reversed, bool reset = false, System.Action endAction = null)
		{
			m_StartByReversed = reversed;
			tweenWrapMode = reset ? WrapMode.Once : WrapMode.ClampForever;
			return Tween (endAction);
		}

		void TweenStop ()
		{
			LeanTween.cancel (gameObject, m_LastTweenId);
			m_LastTweenId = -1;
		}

		bool Tween (System.Action endAction = null)
		{
			Init ();

			TweenStop ();
			LTDescr lt = TweenRun (!m_StartByReversed, tweenTime).setIgnoreTimeScale (ignoreTimeScale).setEase (tweenEaseType);
			if (lt == null) {
				if (endAction != null) {
					endAction ();
				}
				return false;
			}

			m_LastTweenId = lt.uniqueId;

			lt.onComplete = () => {
				switch (tweenWrapMode) {
				case WrapMode.Loop:
					if (m_TweenLoopCount > 0) {
						m_TweenLoopCount--;
					}
					if (m_TweenLoopCount != 0) {
						m_IsInited = false;
						Tween (endAction);
						return;
					}
					break;

				case WrapMode.PingPong:
					if (m_TweenLoopCount > 0) {
						m_TweenLoopCount--;
					}
					if (m_TweenLoopCount != 0) {
						m_StartByReversed = !m_StartByReversed;
						Tween (endAction);
						return;
					}
					break;

				case WrapMode.Once:
					m_IsInited = false;
					Init ();
					break;
				}

				if (endAction != null) {
					endAction ();
				}
			};

			return true;
		}

		protected abstract LTDescr TweenRun (bool reversed, float time);

		#endregion
	}
}
