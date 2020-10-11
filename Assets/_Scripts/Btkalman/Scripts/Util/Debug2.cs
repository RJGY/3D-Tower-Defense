using UnityEngine;

namespace Btkalman.Util {
    public class Debug2 {
        public static void DrawArrow(
                Vector2 start,
                Vector2 end,
                Color color,
                float duration,
                float headLength = 0.1f,
                float headAngle = 15f) {
            Debug.DrawLine(start, end, color, duration);
            var dir = (end - start).normalized * headLength;
            var q = Quaternion.Euler(0f, 0f, headAngle);
            Debug.DrawLine(end - (Vector2)(q * dir), end, color, duration);
            Debug.DrawLine(end - (Vector2)(Quaternion.Inverse(q) * dir), end, color, duration);
        }

        public static void DrawArrowRay(
                Vector2 start,
                Vector2 dir,
                Color color,
                float duration,
                float headLength = 0.1f,
                float headAngle = 15f) {
            DrawArrow(start, start + dir, color, duration, headLength, headAngle);
        }

        public static void DrawBounds(Bounds bounds, Color color, float duration) {
            Vector2[] points = new Vector2[]{
                bounds.min,
                new Vector2(bounds.max.x, bounds.min.y),
                bounds.max,
                new Vector2(bounds.min.x, bounds.max.y),
            };
            for (int i = 0; i < points.Length; i++) {
                for (int j = i + 1; j < points.Length; j++) {
                    Debug.DrawLine(points[i], points[j], color, duration);
                }
            }
        }

        public static void DrawConeRay(
                Vector2 start,
                Vector2 direction,
                float angle,
                Color color,
                float duration,
                int segments = 32) {
            DrawCircleOrCone(true, start, color, duration, direction, angle, segments);
        }

        public static void DrawCircle(
                Vector2 center,
                float radius,
                Color color,
                float duration,
                int segments = 32) {
            DrawCircleOrCone(false, center, color, duration, Vector2.up * radius, 360f, segments);
        }

        private static void DrawCircleOrCone(
                bool cone,
                Vector2 center,
                Color color,
                float duration,
                Vector2 arcDirection,
                float arcAngle,
                int segments) {
            if (arcDirection == Vector2.zero) {
                arcDirection = Vector2.up;
            }

            arcAngle = Mathf.Clamp(arcAngle, 0f, 360f);
            if (arcAngle < 360f) {
                segments *= Mathf.CeilToInt(arcAngle / 360f);
                if (segments % 2 == 1) {
                    // There should always be a point at the end of the arcDir.
                    segments++;
                }
            }

            segments = Mathf.Max(segments, 3);

            // Draw all but the final segment.
            var start = Quaternion.Euler(0f, 0f, -arcAngle / 2) * arcDirection;
            var end = Quaternion.Euler(0f, 0f, arcAngle / 2) * arcDirection;

            var d = start;
            var q = Quaternion.Euler(0f, 0f, arcAngle / segments);
            for (int i = 0; i < segments - 1; i++) {
                var d2 = q * d;
                Debug.DrawLine((Vector3)center + d, (Vector3)center + d2, color, duration);
                d = d2;
            }

            // Draw final segment to end. This avoids rounding errors.
            Debug.DrawLine((Vector3)center + d, (Vector3)center + end, color, duration);

            // For the cone, draw lines from the center to the start and end.
            if (cone) {
                Debug.DrawRay(center, start, color, duration);
                Debug.DrawRay(center, end, color, duration);
            }
        }

        public static void Log(params object[] list) {
            string format = "";
            for (int i = 0; i < list.Length; i++) {
                if (format != "") {
                    format += " ";
                }
                format += "{" + i + "}";
            }
            Debug.LogFormat(format, list);
        }
    }
}