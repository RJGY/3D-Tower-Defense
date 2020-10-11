namespace Btkalman.Util {
    public class Timer {
        public float countdown { get; private set; }

        private float timeValue;

        public Timer(float t) {
            if (t < 0f) {
                throw new System.Exception("Cannot create Timer with time < 0: " + t);
            }
            timeValue = t;
        }

        public Timer Start() {
            countdown = timeValue;
            return this;
        }

        public Timer StartIfNotStarted() {
            if (countdown == 0) {
                countdown = timeValue;
            }
            return this;
        }

        public Timer Cancel() {
            countdown = 0;
            return this;
        }

        public bool Update(float deltaTime) {
            if (countdown == 0) {
                return false;
            }
            countdown -= deltaTime;
            if (countdown < 0f) {
                countdown = 0f;
            }
            return countdown == 0f;
        }

        override public string ToString() {
            return string.Format("Timer(timeValue={0} countdown={1})", timeValue, countdown);
        }
    }
}
