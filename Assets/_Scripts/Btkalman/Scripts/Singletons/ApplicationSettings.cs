using UnityEngine;

namespace Btkalman.Singletons {
    public class ApplicationSettings : MonoBehaviour {
        [SerializeField] [Range(0, 300)] private int targetFrameRate = 60;

        private void Awake() {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
