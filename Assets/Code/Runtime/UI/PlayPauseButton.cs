using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    public class PlayPauseButton : MonoBehaviour
    {
        [SerializeField]
        private Image playImg;
        [SerializeField]
        private Image pauseImg;
    
        private IGameStateService _gameStateService;
        private bool _isPaused;

        private void Start()
        {
            _gameStateService = ServiceProvider.Get<IGameStateService>();
            _gameStateService.GameStateChanged.AddListener(OnGameStateChanged);
        }
    
        public void Toggle()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                playImg.enabled = false;
                pauseImg.enabled = true;
                _gameStateService.PauseGame();
            }
            else
            {
                playImg.enabled = true;
                pauseImg.enabled = false;
                _gameStateService.ResumeGame();
            }
        }

        private void OnGameStateChanged(IGameStateService.State state)
        {
            if (state == IGameStateService.State.Paused && !_isPaused ||
                state == IGameStateService.State.Playing && _isPaused)
            {
                Toggle();
            }
        }
    }
}
