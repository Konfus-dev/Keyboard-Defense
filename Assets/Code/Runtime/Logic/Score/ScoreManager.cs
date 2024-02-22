using System;
using Konfus.Utility.Design_Patterns;
using UnityEngine.Events;

namespace KeyboardDefense.Logic.Score
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public UnityEvent onScoreChanged;
        
        private int _score;
        public int Score => _score;

        private void Start()
        {
            onScoreChanged.Invoke();
        }

        public void AddToScore(int amount)
        {
            _score += amount;
            onScoreChanged.Invoke();
        }
    }
}
