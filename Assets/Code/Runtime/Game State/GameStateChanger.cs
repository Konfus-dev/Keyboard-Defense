using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Game_State
{
    public class GameStateChanger : MonoBehaviour
    {
        [SerializeField]
        private IGameStateService.State stateToChangeTo;
        private IGameStateService _gameStateManager;

        private void Start()
        {
            _gameStateManager = ServiceProvider.Get<IGameStateService>();
        }

        public void ChangeGameState()
        {
            _gameStateManager.GameState = stateToChangeTo;
        }

        public void ChangeGameState(IGameStateService.State newState)
        {
            stateToChangeTo = newState;
            _gameStateManager.GameState = newState;
        }
    }
}