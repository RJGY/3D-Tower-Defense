using UnityEngine;

namespace Btkalman.Singletons {
    public class FrameStepper : MonoBehaviour {
        public bool skipFrame { get; private set; }

        public float deltaTime {
            get {
                if (!enableDebugFrameRate) {
                    return Time.deltaTime;
                }
                if (skipFrame) {
                    Debug.LogWarning("Cannot get deltaTime when skipFrame is true");
                }
                return 1f / debugFrameRate;
            }
            private set { }
        }

        [SerializeField] [Range(0, 300)] private int debugFrameRate = 0;

        private bool enableDebugFrameRate;

        private void Start() {
            enableDebugFrameRate = false;
            skipFrame = false;
        }

        private void Update() {
            if (debugFrameRate == 0) {
                enableDebugFrameRate = false;
                skipFrame = false;
                return;
            }

            skipFrame = enableDebugFrameRate;

            if (Input.GetKeyDown(KeyCode.Backslash)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    enableDebugFrameRate = !enableDebugFrameRate;
                    skipFrame = enableDebugFrameRate;
                } else {
                    skipFrame = false;
                }
            }
        }
    }
}
