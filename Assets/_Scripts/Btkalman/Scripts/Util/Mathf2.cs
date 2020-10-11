using UnityEngine;

namespace Btkalman.Util {
    public class Mathf2 {
        public static void ClampRef(ref float value, float min, float max) {
            value = Mathf.Clamp(value, min, max);
        }

        public static float MaxMagnitude(float a, float b) {
            return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
        }
    }
}
