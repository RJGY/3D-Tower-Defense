using UnityEngine;

namespace Btkalman.Util {
    public class Collider2DUtil {
        public static int RemoveNonOverlappingColliders(
                Collider2D collider,
                int overlapCount,
                Collider2D[] overlapColliders,
                float epsilon) {
            int removeCount = 0;
            for (int i = 0; i < overlapCount; i++) {
                var area = BoundsUtil.IntersectionArea(collider.bounds, overlapColliders[i].bounds);
                if (area < epsilon) {
                    removeCount++;
                } else if (removeCount != 0) {
                    overlapColliders[i - removeCount] = overlapColliders[i];
                }
            }
            return overlapCount - removeCount;
        }

        public static bool HitsFloor(
                Collider2D collider,
                Vector2 distance,
                ContactFilter2D filter,
                RaycastHit2D[] hits,
                float epsilon) {
            var hitCount = collider.Cast(distance, filter, hits, distance.magnitude);
            for (int i = 0; i < hitCount; i++) {
                var hit = hits[i];
                if (hit.normal == Vector2.up &&
                    Mathf.Abs(collider.bounds.max.x - hit.collider.bounds.min.x) > epsilon &&
                    Mathf.Abs(collider.bounds.min.x - hit.collider.bounds.max.x) > epsilon) {
                    return true;
                }
            }
            return false;
        }

        public static RaycastHit2D? GetClosestHitInFront(
                GameObject gameObject, Vector2 forwardDirection, int count, RaycastHit2D[] hits) {
            var forwardPosition = gameObject.transform.position + (Vector3)forwardDirection;
            var backwardPosition = gameObject.transform.position - (Vector3)forwardDirection;
            RaycastHit2D? closestHit = default;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++) {
                var hit = hits[i];
                var hitObject = hit.collider.gameObject;
                if (hitObject == gameObject) {
                    continue;
                }
                // Maybe there is an easier way than to test that forward is closer than back, but
                // that's what I'm going to do anyway.
                var hitPos = hitObject.transform.position;
                if ((hitPos - forwardPosition).magnitude > (hitPos - backwardPosition).magnitude) {
                    continue;
                }
                var distance = (hitPos - gameObject.transform.position).magnitude;
                if (distance < closestDistance) {
                    closestHit = hit;
                    closestDistance = distance;
                }
            }

            return closestHit;
        }
    }
}
