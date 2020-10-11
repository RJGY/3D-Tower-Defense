using UnityEngine;

namespace Btkalman.Util {
    public class BufferedMax {
        private int index = 0;
        private float time = 0f;
        private float[] updateTimes;
        private float[] updateValues;

        public BufferedMax(int bufferSize) {
            this.updateTimes = new float[bufferSize];
            this.updateValues = new float[bufferSize];
        }

        public void LateUpdate(float timeDelta) {
            time += timeDelta;
        }

        public void Offer(float value) {
            updateTimes[index] = time;
            updateValues[index] = value;
            index = (index + 1) % updateTimes.Length;
        }

        public float Max(float bufferTime) {
            return Max(bufferTime, 0f);
        }

        public float Max(float bufferTime, float defaultValue) {
            var index = MaxIndex(bufferTime);
            return index >= 0f ? updateValues[index] : defaultValue;
        }

        private int MaxIndex(float bufferTime) {
            int index = -1;
            float max = 0f;
            for (int i = 0; i < updateTimes.Length; i++) {
                if (updateTimes[i] > 0f && updateTimes[i] > time - bufferTime) {
                    var value = updateValues[i];
                    if (Mathf.Abs(value) > Mathf.Abs(max)) {
                        index = i;
                        max = value;
                    }
                }
            }
            return index;
        }
    }
}
