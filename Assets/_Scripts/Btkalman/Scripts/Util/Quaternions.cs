using UnityEngine;

namespace Btkalman.Util {
    public class Quaternions {
        public static Quaternion Z(float degz) {
            return Quaternion.Euler(0f, 0f, degz);
        }

        public static Quaternion RadZ(float radz) {
            return Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * radz);
        }
    }
}