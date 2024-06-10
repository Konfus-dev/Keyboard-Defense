using KeyboardDefense.Services;
using TMPro;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameOverLayout;
        [SerializeField]
        private TMP_Text scoreText;
        
        private ISceneManager _sceneManager;
        private IScoreManager _scoreManager;

        public void OnTryAgain()
        {
            _sceneManager.ReloadCurrentScene();
        }

        private void Start()
        {
            _sceneManager = ServiceProvider.Get<ISceneManager>();
            _scoreManager = ServiceProvider.Get<IScoreManager>();
            var player = ServiceProvider.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(int currHealth, int maxHealth)
        {
            if (currHealth <= 0)
            {
                gameOverLayout.SetActive(true);
                scoreText.text = _scoreManager.Score.ToString();
            }
        }
    }
}