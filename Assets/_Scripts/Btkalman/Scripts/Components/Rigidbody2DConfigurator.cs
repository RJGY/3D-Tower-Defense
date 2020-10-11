using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Btkalman.Components {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DConfigurator : MonoBehaviour {
        [Serializable]
        public struct Configuration {
            public string Name;
            public float LinearDrag;
            public float GravityScale;
        }

        [SerializeField] private Configuration[] m_configs = null;

        private Rigidbody2D m_rb = null;
        private Configuration m_awakeConfig;

        public void SetConfig(string name) {
            foreach (var config in m_configs) {
                if (config.Name == name) {
                    m_rb.drag = config.LinearDrag;
                    m_rb.gravityScale = config.GravityScale;
                    return;
                }
            }
            Debug.LogWarningFormat("Did not find configuration for config named '{0}'", name);
        }

        public void Reset() {
            m_rb.drag = m_awakeConfig.LinearDrag;
            m_rb.gravityScale = m_awakeConfig.GravityScale;
        }

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();
            m_awakeConfig.LinearDrag = m_rb.drag;
            m_awakeConfig.GravityScale = m_rb.gravityScale;
        }
    }
}
