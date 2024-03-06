using System;
using KeyboardDefense.Score;
using KeyboardDefense.Services;
using TMPro;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        private IScoreManager _scoreManager;
        
        private void Awake()
        {
            _scoreManager = ServiceProvider.Instance.Get<IScoreManager>();
            _scoreManager.OnScoreChanged.AddListener(OnScoreChanged);
        }

        public void OnScoreChanged()
        {
            scoreText.text = $"Score: {_scoreManager.Score}";
        }
    }
}
