using UnityEngine.Events;

namespace KeyboardDefense.Services
{
    public interface IGameStateService : IGameService
    {
        State GameState { get; set; }
        GameStateChangedEvent GameStateChanged { get; }
        void PauseGame();
        void ResumeGame();
        void GameOver();
        void ExitGame();
        
        public enum State
        {
            Playing,
            Paused,
            GameOver,
            Exiting
        }
    }
    
    public class GameStateChangedEvent : UnityEvent<IGameStateService.State>
    {
    }
}