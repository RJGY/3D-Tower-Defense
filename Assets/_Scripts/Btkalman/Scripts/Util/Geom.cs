using UnityEngine;

namespace Btkalman.Util {
    public class Geom {
        // Returns the "point" angle (of which there is 1) and "base" angles (of which there are 2) of
        // an isosceles triangle given its "base" length (of which there is 1) and "side" lengths
        // (of which there are 2). I don't know the mathematical names. Angles are returned in radians.
        public static void SolveIsoscelesAngles(
            float baseLength, float sideLength, out float pointRad, out float baseRad) {
            baseRad = Mathf.Acos(baseLength / 2f / sideLength);
            pointRad = Mathf.PI - 2f * baseRad;
        }
    }
}
