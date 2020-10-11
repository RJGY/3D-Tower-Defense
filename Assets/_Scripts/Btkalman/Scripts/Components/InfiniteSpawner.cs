using System.Collections.Generic;
using UnityEngine;

namespace Btkalman.Components {
    public class InfiniteSpawner : MonoBehaviour {
        [SerializeField] private ChildCollider2D m_childPrefab = null;
        [SerializeField] private float m_spawnWidth = 1f;
        [SerializeField] private float m_spawnHeight = 1f;
        [SerializeField] private int m_renderAhead = 5;
        [SerializeField] private int m_renderBehind = 1;

        private List<ChildCollider2D> m_childColliders = new List<ChildCollider2D>();
        private List<ChildCollider2D> m_childCollidersBehind = new List<ChildCollider2D>();

        public float GetWidth() {
            return m_spawnWidth;
        }

        private void OnChildTriggerEnter2D(ChildCollider2D.Trigger trigger) {
            int index = m_childColliders.IndexOf(trigger.Child);
            int endOffset = m_childColliders.Count - index - 1;
            for (int i = endOffset; i < m_renderAhead; i++) {
                Spawn();
            }
        }

        private void Start() {
            for (int i = 0; i < m_renderAhead; i++) {
                Spawn();
            }
            for (int i = 0; i < m_renderBehind; i++) {
                Spawn(true);
            }
        }

        private void OnDestroy() {
            m_childColliders.Clear();
            m_childCollidersBehind.Clear();
        }

        private void Spawn(bool behind = false) {
            // TODO: Recycle these.
            var child = GameObject.Instantiate(m_childPrefab, transform);
            // Position above the last spawned child segment, or at (0, 0) if
            // there are no children.
            // Note that we make sure the y axis visual position starts at 0 on
            // the bottom, so the transform y needs to add height / 2.
            var colliders = behind ? m_childCollidersBehind : m_childColliders;
            float y = (colliders.Count * m_spawnHeight) + m_spawnHeight / 2f;
            if (behind) {
                y = -y;
            }
            child.transform.position = new Vector2(0, y);
            child.transform.localScale = new Vector2(m_spawnWidth, m_spawnHeight);
            colliders.Add(child);
        }
    }
}
