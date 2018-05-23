using UnityEngine;
using System.Collections;

namespace CocoPlay
{
	public class CocoMath
	{
		#region Common

		/// <summary>
		/// Is two values is same sign.
		/// </summary>

		public static bool IsSameSign (float f1, float f2)
		{
			return (f1 < 0 && f2 < 0) || (f1 > 0 && f2 > 0) || (f1 == 0 && f2 == 0);
		}

		/// <summary>
		/// Is two values is same approximately.
		/// </summary>

		public static bool Approximately (float v1, float v2, float errorRange = 0.0001f)
		{
			return Mathf.Abs (v1 - v2) <= errorRange;
		}

		public static bool Approximately (Vector2 v1, Vector2 v2, float errorRange = 0.0001f)
		{
			return Approximately (v1.x, v2.x, errorRange) &&
				Approximately (v1.y, v2.y, errorRange);
		}

		public static bool Approximately (Vector3 v1, Vector3 v2, float errorRange = 0.0001f)
		{
			return Approximately (v1.x, v2.x, errorRange) &&
				Approximately (v1.y, v2.y, errorRange) &&
				Approximately (v1.z, v2.z, errorRange);
		}

		#endregion


		#region Angle

		/// <summary>
		/// Ensure that the angle is within -180 to 180 range.
		/// </summary>

		public static float WrapAngle (float angle)
		{
			while (angle > 180f)
				angle -= 360f;
			while (angle < -180f)
				angle += 360f;
			return angle;
		}

		#endregion


		#region Random In Range

		/// <summary>
		/// random extends.
		/// </summary>

		public static int RandomInRange (int min, int max)
		{
			if (min < max)
				return Random.Range (min, max);

			return min;
		}

		public static float RandomInRange (float min, float max)
		{
			if (min < max)
				return Random.Range (min, max);

			return min;
		}

		public static Vector2 RandomInRange (Vector2 min, Vector2 max)
		{
			Vector2 vec = Vector2.zero;

			vec.x = min.x < max.x ? Random.Range (min.x, max.x) : Random.Range (max.x, min.x);
			vec.y = min.y < max.y ? Random.Range (min.y, max.y) : Random.Range (max.y, min.y);

			return vec;
		}

		public static Vector3 RandomInRange (Vector3 min, Vector3 max)
		{
			Vector3 vec = Vector3.zero;

			vec.x = min.x < max.x ? Random.Range (min.x, max.x) : Random.Range (max.x, min.x);
			vec.y = min.y < max.y ? Random.Range (min.y, max.y) : Random.Range (max.y, min.y);
			vec.z = min.z < max.z ? Random.Range (min.z, max.z) : Random.Range (max.z, min.z);

			return vec;
		}

		#endregion


		#region Divde

		/// <summary>
		/// Divide the specified vec1 and vec2.
		/// </summary>

		public static Vector3 Divide (Vector3 vec1, Vector3 vec2)
		{
			return new Vector3 (vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
		}

		/// <summary>
		/// Divide the specified f(float) and vec2.
		/// </summary>
		/// 
		public static Vector3 Divide (float f, Vector3 vec2)
		{
			return new Vector3 (f / vec2.x, f / vec2.y, f / vec2.z);
		}

		#endregion
		
		
		#region Scale

		public static Rect Scale (Rect rect, float scale)
		{
			rect.position *= scale;
			rect.size *= scale;
			return rect;
		}
		
		public static Rect Scale (Rect rect, Vector2 scale)
		{
			rect.position = Vector2.Scale (rect.position, scale);
			rect.size = Vector2.Scale (rect.size, scale);
			return rect;
		}
		
		public static Rect Scale (Rect rect, Rect scale)
		{
			rect.position = Vector2.Scale (rect.position, scale.position);
			rect.size = Vector2.Scale (rect.size, scale.size);
			return rect;
		}

		#endregion

		
		#region Round

		public static Vector2 Round (Vector2 vec)
		{
			vec.x = Mathf.Round (vec.x);
			vec.y = Mathf.Round (vec.y);
			return vec;
		}
		
		public static Vector3 Round (Vector3 vec)
		{
			vec.x = Mathf.Round (vec.x);
			vec.y = Mathf.Round (vec.y);
			vec.z = Mathf.Round (vec.z);
			return vec;
		}
		
		public static Rect Round (Rect rect)
		{
			rect.position = Round (rect.position);
			rect.size = Round (rect.size);
			return rect;
		}

		#endregion
	}
}
