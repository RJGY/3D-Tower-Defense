using UnityEngine;

namespace Btkalman.Util {
    class BoundsUtil {
        // Calculates the area of intersection between two Bounds. Useful for making physics decisions
        // when you don't want to allocate a whole new Bounds object.
        public static float IntersectionArea(Bounds a, Bounds b) {
            Vector2 min = new Vector2(
                Mathf.Max(a.min.x, b.min.x), Mathf.Max(a.min.y, b.min.y));
            Vector2 max = new Vector2(
                Mathf.Min(a.max.x, b.max.x), Mathf.Min(a.max.y, b.max.y));
            if (max.x < min.x || max.y < min.y) {
                return 0f;
            }
            return (max.x - min.x) * (max.y - min.y);
        }
    }
}
