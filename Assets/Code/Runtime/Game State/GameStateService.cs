using KeyboardDefense.Services;
using UnityEngine.Events;

namespace KeyboardDefense.UI
{
    // TODO: Finish this to use for pause/unpause logic
    public class GameStateService : GameService<IGameStateService>, IGameStateService
    {
        public GameStateChangedEvent GameStateChanged { get; } = new();

        private void Awake()
        {
        }
        
        public enum State
        {
            Playing,
            Paused,
            GameOver
        }
    }
    
    public class GameStateChangedEvent : UnityEvent<GameStateService.State>
    {
    }
}