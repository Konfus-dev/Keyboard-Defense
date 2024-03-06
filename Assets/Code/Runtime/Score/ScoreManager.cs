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

        private void Start()
        {
            onScoreChanged.Invoke();
        }

        public void AddToScore(int amount)
        {
            score += amount;
            onScoreChanged.Invoke();
        }
    }
}
