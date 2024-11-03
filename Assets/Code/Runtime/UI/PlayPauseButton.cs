using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Toggle))]
    public class PlayPauseButton : MonoBehaviour
    {
        [SerializeField]
        private Image playImg;
        [SerializeField]
        private Image pauseImg;

        private Toggle _toggle;
        private IGameStateService _gameStateService;
        private bool _isPaused;
        private bool _isToggling;

        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _gameStateService = ServiceProvider.Get<IGameStateService>();
            _gameStateService.GameStateChanged.AddListener(OnGameStateChanged);
        }
    
        public void Toggle()
        {
            if (_isToggling) return;
            
            _isToggling = true;
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                playImg.enabled = false;
                pauseImg.enabled = true;
                _toggle.isOn = true;
                _gameStateService.PauseGame();
            }
            else
            {
                playImg.enabled = true;
                pauseImg.enabled = false;
                _toggle.isOn = false;
                _gameStateService.ResumeGame();
            }
            _isToggling = false;
        }

        private void OnGameStateChanged(IGameStateService.State state)
        {
            if (state == IGameStateService.State.GameOver)
            {
                _toggle.gameObject.SetActive(false);
            }
            if (state == IGameStateService.State.Paused && !_isPaused ||
                state == IGameStateService.State.Playing && _isPaused)
            {
                Toggle();
            }
        }
    }
}
