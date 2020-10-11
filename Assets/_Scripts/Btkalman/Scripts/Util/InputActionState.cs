using UnityEngine;
using UnityEngine.InputSystem;

namespace Btkalman.Util {
    class InputActionState {
        public bool Hold { get; private set; } = false;

        private InputAction action;
        private float startedTimer = float.NaN;

        public InputActionState(InputAction action) {
            this.action = action;
            action.started += OnStarted;
            action.canceled += OnCanceled;
        }

        public void LateUpdate(float timeDelta) {
            if (!float.IsNaN(startedTimer)) {
                startedTimer += timeDelta;
            }
        }

        public InputActionState Destroy() {
            action.started -= OnStarted;
            action.canceled -= OnCanceled;
            return null;
        }

        public bool DidStart() {
            return DidStartBuffered(0f);
        }

        public bool DidStartBuffered(float bufferTime) {
            bool started = false;
            if (!float.IsNaN(startedTimer)) {
                started = startedTimer <= bufferTime;
            }
            startedTimer = float.NaN;
            return started;
        }

        private void OnStarted(InputAction.CallbackContext context) {
            Hold = true;
            startedTimer = 0f;
        }

        private void OnCanceled(InputAction.CallbackContext context) {
            Hold = false;
        }
    }
}
