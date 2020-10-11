using UnityEngine;

namespace Btkalman.Util {
    public class Floats {
        public static float Quantize(float f, float accuracy) {
            return accuracy * Mathf.Round(f / accuracy);
        }

        public static int Compare(float f, float g) {
            return f == g ? 0 : f < g ? -1 : 1;
        }

        public static bool EqualsEpsilon(float a, float b, float epsilon = float.Epsilon) {
            if (epsilon < 0) {
                throw new System.Exception("epsilon cannot be < 0");
            }
            float d = a - b;
            return d < epsilon && d > -epsilon;
        }

        public static float Progress(float value, float min, float max) {
            if (min >= max) {
                Debug.LogWarningFormat("min {0} cannot be >= max {1}", min, max);
                // Return 1f here as a safer default value than 0f for callers that probably want
                // Progress to be used for... progress. Returning 0f might freeze Unity.
                return 1f;
            }
            if (value < min) {
                Debug.LogWarningFormat("value {0} cannot be < min {1}", value, min);
                return 0f;
            }
            if (value > max) {
                Debug.LogWarningFormat("value {0} cannot be > max {1}", value, max);
                return 1f;
            }
            return (value - min) / (max - min);
        }
    }
}