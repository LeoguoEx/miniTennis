using UnityEngine;
using System.Collections;


namespace CocoPlay
{
	public class CocoRange<T>
	{
		public CocoRange (T from, T to)
		{
			m_From = from;
			m_To = to;
		}

		[SerializeField]
		T m_From;

		public T From {
			get {
				return m_From;
			}
			set {
				m_From = value;
			}
		}

		[SerializeField]
		T m_To;

		public T To {
			get {
				return m_To;
			}
			set {
				m_To = value;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[CocoRange: From={0}, To={1}]", From, To);
		}
	}

	[System.Serializable]
	public class CocoIntRange : CocoRange<int>
	{
		public CocoIntRange (int from, int to) : base (from, to)
		{
		}
	}

	[System.Serializable]
	public class CocoFloatRange : CocoRange<float>
	{
		public CocoFloatRange (float from, float to) : base (from, to)
		{
		}
	}

	[System.Serializable]
	public class CocoVector3Range : CocoRange<Vector3>
	{
		public CocoVector3Range (Vector3 from, Vector3 to) : base (from, to)
		{
		}
	}

	[System.Serializable]
	public class CocoColorRange : CocoRange<Color>
	{
		public CocoColorRange (Color from, Color to) : base (from, to)
		{
		}
	}

}
