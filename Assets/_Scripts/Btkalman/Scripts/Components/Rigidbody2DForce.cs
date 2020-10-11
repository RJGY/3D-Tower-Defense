using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Btkalman.Components {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DForce : MonoBehaviour {
        public Vector2 force;

        private Rigidbody2D m_rb;

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            if (force != Vector2.zero) {
                m_rb.AddForce(force * Time.fixedDeltaTime);
            }
        }
    }
}