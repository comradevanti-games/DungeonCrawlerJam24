using System;

namespace DGJ24.Score
{
    public interface IScoreTracker
    {
        public event Action<int> ScoreUpdated;

        public int Score { get; }

        public void Add(int amount);
    }
}
