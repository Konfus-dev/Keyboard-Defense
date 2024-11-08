using KeyboardDefense.Services;
using TMPro;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        private IScoreManager _scoreManager;
        
        private void Start()
        {
            _scoreManager = ServiceProvider.Get<IScoreManager>();
            _scoreManager.OnScoreChanged.AddListener(OnScoreChanged);
        }

        public void OnScoreChanged()
        {
            scoreText.text = $"Score: {_scoreManager.Score}";
        }
    }
}
