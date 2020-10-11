using UnityEngine;
using Btkalman.Singletons;

namespace Btkalman.Components {
    public class ScoreComponent : MonoBehaviour, ScoreSystem.Listener {
        public interface Messages {
            void OnScoreDidChange(ScoreSystem.Score score);
        }

        [SerializeField] private string m_name = null;
        [SerializeField] private ScoreSystem m_scoreSystem = null;

        public ScoreSystem.Score GetScore() {
            return m_scoreSystem.GetScore(m_name);
        }

        public void SetScore(float score) {
            m_scoreSystem.SetScore(score, m_name);
        }

        public void SetMinScore(float score) {
            m_scoreSystem.SetMinScore(score, m_name);
        }

        public void AddScore(float score) {
            m_scoreSystem.AddScore(score, m_name);
        }

        private void Start() {
            m_scoreSystem.AddListener(this);
        }

        public void OnScoreChange(ScoreSystem.Score score) {
            SendMessage("OnScoreDidChange", GetScore());
        }
    }
}
