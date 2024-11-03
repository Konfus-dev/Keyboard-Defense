using KeyboardDefense.Serialization;
using KeyboardDefense.Services;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Score
{
    public class ScoreManager : GameService<IScoreManager>, IScoreManager
    {
        [SerializeField]
        private UnityEvent onScoreChanged;
        public UnityEvent OnScoreChanged => onScoreChanged;
        
        [SerializeField, ReadOnly]
        private int score;
        public int Score => score;
        
        private IDataSaverLoader _dataSaverLoader;

        private void Start()
        {
            _dataSaverLoader = ServiceProvider.Get<IDataSaverLoader>();
            var gameStateService = ServiceProvider.Get<IGameStateService>();
            var sceneManager = ServiceProvider.Get<ISceneManager>();
            sceneManager.ChangedScene.AddListener(OnSceneChanged);
            gameStateService.GameStateChanged.AddListener(OnGameStateChanged);
            onScoreChanged.Invoke();
        }

        private void OnSceneChanged()
        {
            SaveHighScore();
        }

        private void OnGameStateChanged(IGameStateService.State state)
        {
            if (state is IGameStateService.State.Exiting or IGameStateService.State.GameOver)
            {
                SaveHighScore();
            }
        }

        private void SaveHighScore()
        {
            var highScore = _dataSaverLoader.LoadData<HighScoreSaveData>("HighScore")?.highScore ?? 0;
            if (score > highScore) _dataSaverLoader.SaveData("HighScore", new HighScoreSaveData(){ highScore = score });
        }

        public void AddToScore(int amount)
        {
            score += amount;
            onScoreChanged.Invoke();
        }
    }
}
