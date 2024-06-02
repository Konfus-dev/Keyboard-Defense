using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        private ISceneManager _sceneManager;

        public void OnTryAgain()
        {
            _sceneManager.ReloadCurrentScene();
        }
        
        private void Start()
        {
            _sceneManager = ServiceProvider.Get<ISceneManager>();
            var player = ServiceProvider.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(int currHealth, int maxHealth)
        {
            if (currHealth <= 0)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}