using UnityEngine;

namespace Btkalman.Components {
    [RequireComponent(typeof(Collider2D))]
    public class ChildCollider2D : MonoBehaviour {
        public class Trigger {
            public ChildCollider2D Child;
            public Collider2D ChildCollider;
            public Collider2D OtherCollider;

            public Trigger(ChildCollider2D child, Collider2D otherCollider) {
                Child = child;
                ChildCollider = child.m_collider2D;
                OtherCollider = otherCollider;
            }
        }

        private Collider2D m_collider2D = null;

        private void Awake() {
            m_collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            transform.parent.gameObject.SendMessage(
                "OnChildTriggerEnter2D",
                new Trigger(this, collider));
        }
    }
}
