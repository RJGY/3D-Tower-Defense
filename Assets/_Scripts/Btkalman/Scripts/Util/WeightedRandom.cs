using UnityEngine;

namespace Btkalman.Util {
    public class WeightedRandom {
        private float[] m_weights;
        private float m_decay;

        public WeightedRandom(int n, float decay) {
            m_weights = new float[n];
            for (int i = 0; i < n; i++) {
                m_weights[i] = 1f / (float)n;
            }
            m_decay = decay;
            SanityCheck();
        }

        public int Generate() {
            SanityCheck();
            int value = m_weights.Length - 1;
            float r = Random.value;
            float sum = 0;
            for (int i = 0; i < m_weights.Length; i++) {
                sum += m_weights[i];
                if (r < sum) {
                    value = i;
                    break;
                }
            }
            if (m_weights.Length > 1) {
                float adjust = m_decay * m_weights[value];
                m_weights[value] -= adjust;
                for (int i = 0; i < m_weights.Length; i++) {
                    if (i != value) {
                        m_weights[i] += adjust / (m_weights.Length - 1);
                    }
                }
            }
            SanityCheck();
            return value;
        }

        public int ItemCount() {
            return m_weights.Length;
        }

        public void AddItem() {
            SanityCheck();
            float[] weights = new float[m_weights.Length + 1];
            float value = 1f / (float)weights.Length;
            weights[weights.Length - 1] = value;
            for (int i = 0; i < weights.Length - 1; i++) {
                weights[i] = m_weights[i] - value / ((float)weights.Length - 1f);
            }
            m_weights = weights;
            SanityCheck();
        }

        private void SanityCheck() {
            float sum = 0;
            foreach (var weight in m_weights) {
                sum += weight;
            }
            if (Floats.Quantize(sum - 1f, 0.00001f) != 0f) {
                Debug.LogWarningFormat("Failed WeightedRandom sanity check, total is {0}", sum);
            }
        }

        override public string ToString() {
            string weightsString = "[";
            foreach (var w in m_weights) {
                if (weightsString != "[") {
                    weightsString += ", ";
                }
                weightsString += w;
            }
            return weightsString + "]";
        }
    }
}