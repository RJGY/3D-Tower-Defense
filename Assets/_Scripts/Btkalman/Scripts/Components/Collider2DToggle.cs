using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Btkalman.Components {
    public class Collider2DToggle : MonoBehaviour {
        [Serializable]
        public class Option {
            public string Name;
            public Collider2D Collider;
        }

        [SerializeField] private Option[] m_colliders = null;

        private int m_colliderIndex = 0;

        public void SetEnabled(string name) {
            for (int i = 0; i < m_colliders.Length; i++) {
                if (m_colliders[i].Name == name) {
                    m_colliderIndex = i;
                    UpdateEnabled();
                    return;
                }
            }
            Debug.LogWarningFormat("No collider named {0}", name);
        }

        public Collider2D GetEnabled() {
            return m_colliders[m_colliderIndex].Collider;
        }

        public bool IsTouching(Collider2D collider, string name) {
            for (int i = 0; i < m_colliders.Length; i++) {
                var namedCollider = m_colliders[i];
                if (namedCollider.Name == name) {
                    return namedCollider.Collider.IsTouching(collider);
                }
            }
            Debug.LogWarningFormat("No collider named {0}", name);
            return false;
        }

        private void Awake() {
            UpdateEnabled();
        }

        private void UpdateEnabled() {
            for (int i = 0; i < m_colliders.Length; i++) {
                var collider = m_colliders[i];
                var enabled = i == m_colliderIndex;
                if (collider.Collider.enabled != enabled) {
                    collider.Collider.enabled = enabled;
                }
            }
        }
    }
}
