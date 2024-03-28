using System;
using UnityEngine;

namespace DGJ24.Score
{
    internal class ScoreTracker : MonoBehaviour, IScoreTracker
    {
        public event Action<int>? ScoreUpdated;

        public int Score { get; set; }

        public void Add(int amount)
        {
            Score += amount;
            ScoreUpdated?.Invoke(Score);
        }
    }
}
