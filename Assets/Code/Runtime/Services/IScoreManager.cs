﻿using UnityEngine.Events;

namespace KeyboardDefense.Services
{
    public interface IScoreManager : IGameService
    {
        int Score { get; }
        void AddToScore(int amount);
        UnityEvent OnScoreChanged { get; }
    }
}