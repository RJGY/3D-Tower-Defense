using UnityEngine;

namespace Btkalman.Components {
    [RequireComponent(typeof(Collider2D))]
    public class Rigidbody2DRubberBand : MonoBehaviour {
        public Rigidbody2D target;
        public float strength = 1f;
        public bool infiniteDistance = false;
        public float maximumDistance = 10f;

        private Collider2D m_collider;

        private void Awake() {
            m_collider = GetComponent<Collider2D>();
        }

        private void FixedUpdate() {
            if (!target) {
                return;
            }
            RaycastHit2D hit;
            bool right = false;
            if (!RaycastFromDirection(Vector2.left, out hit) &&
                !(right = RaycastFromDirection(Vector2.right, out hit))) {
                return;
            }
            var force = new Vector2(hit.distance * strength * Time.fixedDeltaTime, 0f);
            if (!right) {
                force *= -1;
            }
            target.AddForce(force);
        }

        private bool RaycastFromDirection(Vector2 dir, out RaycastHit2D hit) {
            var results = Physics2D.RaycastAll(
                target.position,
                dir,
                infiniteDistance ? Mathf.Infinity : maximumDistance);
            foreach (var result in results) {
                if (result.collider == m_collider) {
                    hit = result;
                    return true;
                }
            }
            hit = default(RaycastHit2D);
            return false;
        }
    }
}