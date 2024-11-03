using KeyboardDefense.Services;
using TMPro;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class GameOverDisplay : GameService<IGameOverUI>, IGameOverUI
    {
        [SerializeField]
        private GameObject gameOverLayout;
        [SerializeField]
        private TMP_Text finalScoreText;
        
        private IScoreManager _scoreManager;

        public void Show()
        {
            gameOverLayout.SetActive(true);
            finalScoreText.text = _scoreManager.Score.ToString();
        }

        private void Start()
        {
            _scoreManager = ServiceProvider.Get<IScoreManager>();
        }
    }
}