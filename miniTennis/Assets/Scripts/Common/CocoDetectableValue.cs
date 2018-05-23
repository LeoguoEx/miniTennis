using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public class CocoDetectableValue<T>
	{
		public CocoDetectableValue (T obj)
		{
			m_Value = obj;
			m_RecordedValue = ValueRecord;
		}


		#region Value

		public System.Action<T> onValueChanged = null;

		[SerializeField]
		T m_Value;

		public T Value {
			get {
				return m_Value;
			}
			set {
				m_Value = value;
				DetectChange ();
			}
		}

		T m_RecordedValue;

		protected virtual T ValueRecord {
			get {
				return Value;
			}
		}

		#endregion


		#region Detect Change

		bool DetectChange ()
		{
			if (IsDifferent (m_Value, m_RecordedValue)) {
				//Debug.LogError (m_RecordedValue + " -> " + m_Value);
				m_RecordedValue = ValueRecord;
				if (onValueChanged != null) {
					onValueChanged (m_Value);
				}
				return true;
			}

			return false;
		}

		#endregion


		#region Compare Differ

		bool IsDifferent (T value1, T value2)
		{
			if (value1 == null && value2 == null) {
				return false;
			}

			if (value1 == null || value2 == null) {
				return true;
			}

			return IsNonNullDifferent (value1, value2);
		}

		protected virtual bool IsNonNullDifferent (T value1, T value2)
		{
			return !value1.Equals (value2); 
		}

		#endregion
	}


	[System.Serializable]
	public class CocoDetectableIntValue : CocoDetectableValue<int>
	{
		public CocoDetectableIntValue (int value) : base (value)
		{
		}
	}

	[System.Serializable]
	public class CocoDetectableFloatValue : CocoDetectableValue<float>
	{
		float m_ErrorRange = 0f;

		public CocoDetectableFloatValue (float value, float errorRange = 0.0001f) : base (value)
		{
			m_ErrorRange = Mathf.Abs (errorRange);
		}

		protected override bool IsNonNullDifferent (float value1, float value2)
		{
			return !CocoMath.Approximately (value1, value2, m_ErrorRange);
		}
	}

	[System.Serializable]
	public class CocoDetectableBoolValue : CocoDetectableValue<bool>
	{
		public CocoDetectableBoolValue (bool value) : base (value)
		{
		}
	}

	[System.Serializable]
	public class CocoDetectableVector3Value : CocoDetectableValue<Vector3>
	{
		float m_ErrorRange = 0f;

		public CocoDetectableVector3Value (Vector3 value, float errorRange = 0.0001f) : base (value)
		{
			m_ErrorRange = Mathf.Abs (errorRange);
		}

		protected override bool IsNonNullDifferent (Vector3 value1, Vector3 value2)
		{
			return !CocoMath.Approximately (value1, value2, m_ErrorRange);
		}
	}

}
