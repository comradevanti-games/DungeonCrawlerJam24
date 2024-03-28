using System;

namespace DGJ24.Score {

	public interface IScoreTracker {

		public int Score { get; set; }

		public event Action<int> ScoreUpdated;
		public void Add(int amount);

	}

}