using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Btkalman.Util;

namespace Btkalman.Singletons {
    public class ScoreSystem : MonoBehaviour {
        public interface Listener {
            void OnScoreChange(Score score);
        }

        public class Score {
            public string Name { get; private set; }
            public float Value { get; private set; }

            public Score(string name, float value) {
                this.Value = value;
                this.Name = name;
            }

            public int Int() {
                return (int)Mathf.Round(Value);
            }

            public string Format() {
                int intValue = Int();
                string format = "{0}";
                if (intValue >= 1e6) {
                    format = "{0:0,000,000}";
                } else if (intValue >= 1e3) {
                    format = "{0:0,000}";
                }
                return string.Format(format, intValue);
            }
        }

        [Header("Text UI")]
        [SerializeField] private int m_highScoreCount = 3;
        [SerializeField] private string m_noScorePlaceholder = "";
        [SerializeField] private Text[] m_gameScoreTexts = null;
        [SerializeField] private Text m_highScoresText = null;

        [Header("Prefs")]
        [SerializeField] private string m_playerPrefsPrefix = "Btkalman.Systems.ScoreSystem";
        [SerializeField] private bool m_clearOnStart = false;

        // Make sure there is only a single instance of the score system.
        private static ScoreSystem instance = null;

        private Dictionary<string, Score> m_currentScores = new Dictionary<string, Score>();
        private List<Score> m_oldScores = new List<Score>();
        private bool m_didSaveCurrentScores = false;
        private HashSet<Listener> m_listeners = new HashSet<Listener>();

        public Score GetScore(string name = "") {
            if (!m_currentScores.ContainsKey(name)) {
                return new Score(name, 0);
            }
            return m_currentScores[name];
        }

        public void SetScore(float score, string name = "") {
            var scoreObject = new Score(name, score);
            m_currentScores[name] = scoreObject;
            var formatScore = scoreObject.Format();
            foreach (var gameScoreText in m_gameScoreTexts) {
                gameScoreText.text = formatScore;
            }
            foreach (var listener in m_listeners) {
                listener.OnScoreChange(scoreObject);
            }
        }

        public bool SetMinScore(float score, string name = "") {
            if (score <= GetScore(name).Value) {
                return false;
            }
            SetScore(score, name);
            return true;
        }

        public void AddScore(float score, string name = "") {
            SetScore(GetScore(name).Value + score, name);
        }

        public void GameOver() {
            if (!m_didSaveCurrentScores) {
                foreach (var nameScore in m_currentScores) {
                    m_oldScores.Add(nameScore.Value);
                }
                var sze = new ScoreSystemSerialization.Scores(m_oldScores);
                PlayerPrefs.SetString(PrefKey("Scores"), JsonUtility.ToJson(sze));
                PlayerPrefs.Save();
                m_didSaveCurrentScores = true;
            }
        }

        public void NewGame() {
            var scores = new Dictionary<string, Score>(m_currentScores);
            foreach (var score in scores) {
                if (score.Value.Value > 0f) {
                    // Reset all non-0 scores to 0 so that the listeners fire.
                    SetScore(0, score.Key);
                }
            }
            m_currentScores.Clear();
            m_didSaveCurrentScores = false;
        }

        public int HighScoreCount() {
            return m_highScoreCount;
        }

        public List<Score> GetHighScores() {
            var scores = new List<Score>(m_oldScores);
            scores.Sort((a, b) => b.Int() - a.Int());
            return scores.GetRange(0, Mathf.Min(m_highScoreCount, scores.Count));
        }

        public bool GetScoreRank(float score, out int rank, out int outOf) {
            var scores = new List<Score>(m_oldScores);
            rank = 0;
            outOf = scores.Count;
            scores.Sort((a, b) => b.Int() - a.Int());
            for (int i = 0; i < outOf; i++) {
                if (Floats.EqualsEpsilon(score, scores[i].Value)) {
                    rank = i;
                    return true;
                }
            }
            return false;
        }

        public void ShowHighScoreText() {
            if (m_highScoresText) {
                var highScoreText = "";
                var highScores = GetHighScores();
                foreach (var score in GetHighScores()) {
                    if (highScoreText != "") {
                        highScoreText += "\n";
                    }
                    highScoreText += score.Format();
                    if (score.Name != "") {
                        highScoreText += " \u2022 " + score.Name;
                    }
                }
                for (int i = highScores.Count; i < m_highScoreCount; i++) {
                    if (highScoreText != "") {
                        highScoreText += "\n";
                    }
                    highScoreText += m_noScorePlaceholder;
                }
                m_highScoresText.text = highScoreText;
            }
        }

        public void AddListener(Listener listener) {
            m_listeners.Add(listener);
        }

        public void RemoveListener(Listener listener) {
            m_listeners.Remove(listener);
        }

        private void Awake() {
            if (instance != null) {
                Debug.LogWarning("There is already a ScoreSystem instance");
                Component.Destroy(this);
            }
        }

        private void Start() {
            var scoreKey = PrefKey("Scores");
            if (m_clearOnStart) {
                PlayerPrefs.DeleteKey(scoreKey);
            } else if (PlayerPrefs.HasKey(scoreKey)) {
                var scoreJson = PlayerPrefs.GetString(scoreKey);
                var sze = JsonUtility.FromJson<ScoreSystemSerialization.Scores>(scoreJson);
                m_oldScores = sze.GetScoreObjects();
            }
            foreach (var gameScoreText in m_gameScoreTexts) {
                gameScoreText.text = "";
            }
        }

        private void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }

        private string PrefKey(string name) {
            return m_playerPrefsPrefix + "/" + name;
        }
    }

    class ScoreSystemSerialization {
        [Serializable]
        public class Scores {
            public Scores(List<ScoreSystem.Score> scoreObjs) {
                scores = new SerializableScore[scoreObjs.Count];
                for (int i = 0; i < scores.Length; i++) {
                    scores[i] = new SerializableScore(scoreObjs[i]);
                }
            }

            public List<ScoreSystem.Score> GetScoreObjects() {
                var scoreObjs = new List<ScoreSystem.Score>();
                for (int i = 0; i < scores.Length; i++) {
                    scoreObjs.Add(new ScoreSystem.Score(scores[i].Name, scores[i].Value));
                }
                return scoreObjs;
            }

            public SerializableScore[] scores;
        }

        [Serializable]
        public class SerializableScore {
            public SerializableScore(ScoreSystem.Score scoreObj) {
                Name = scoreObj.Name;
                Value = scoreObj.Value;
            }

            public string Name;
            public float Value;
        }
    }
}