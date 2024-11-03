using System;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Game_State
{
    public class GameStateManager : GameService<IGameStateService>, IGameStateService
    {
        public GameStateChangedEvent GameStateChanged { get; } = new();

        public IGameStateService.State GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                GameStateChanged.Invoke(_gameState);
            }
        }
        
        private IGameStateService.State _gameState;
        private IGameOverUI _gameOverUI;
        private IPauseUI _pauseUI;
        private ISceneManager _sceneManager;

        private void Start()
        {
            _sceneManager = ServiceProvider.Get<ISceneManager>();
            _gameOverUI = ServiceProvider.Get<IGameOverUI>();
            _pauseUI = ServiceProvider.Get<IPauseUI>();
            GameState = IGameStateService.State.Playing;
            GameStateChanged.AddListener(OnGameStateChanged);
        }

        public void PauseGame()
        {
            GameState = IGameStateService.State.Paused;
        }

        public void ResumeGame()
        {
            GameState = IGameStateService.State.Playing;
        }

        public void GameOver()
        {
            GameState = IGameStateService.State.GameOver;
        }

        public void ExitGame()
        {
            GameState = IGameStateService.State.Exiting;
        }

        private void OnGameStateChanged(IGameStateService.State state)
        {
            switch (state)
            {
                case IGameStateService.State.Playing:
                    // Instead of using timescale we just have things that need to stop listen to game state service and pause whatever they are doing!
                    //Time.timeScale = 1;
                    _pauseUI.Hide();
                    break;
                case IGameStateService.State.Paused:
                    // Instead of using timescale we just have things that need to stop listen to game state service and pause whatever they are doing!
                    //Time.timeScale = 0;
                    _pauseUI.Show();
                    break;
                case IGameStateService.State.GameOver:
                    _gameOverUI.Show();
                    break;
                case IGameStateService.State.Exiting:
                    _sceneManager.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}