using UnityEngine;

// static utility class
public static class Util {

	public const float very_small = 0.0001f;

	public static float Sign(float check) {
		if (check > 0.0f) return 1.0f;
		if (check < 0.0f) return -1.0f;
		return 0.0f;
	}

	public static float MoveToward(float from, float to, float delta) {
		float direction = Sign(to - from) * delta;
		if (direction > 0.0f) return Mathf.Min(from + direction, to);
		if (direction < 0.0f) return Mathf.Max(from + direction, to);
		return to;
	}

}
