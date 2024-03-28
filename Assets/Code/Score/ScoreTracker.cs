using System;
using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.Score {

	internal class ScoreTracker : MonoBehaviour, IScoreTracker {

		public int Score { get; set; }
		public event Action<int>? ScoreUpdated;

		private IActionDirector? ActorDirector { get; set; }

		private void Awake() {
			ActorDirector = Singletons.Get<IActionDirector>();
			ActorDirector.LootCollected += Add;
			ActorDirector.PlayerDied += OnPlayerDeath;
		}

		public void Add(int amount) {
			Score += amount;
			ScoreUpdated?.Invoke(Score);
		}

		private void OnPlayerDeath() {
			PlayerPrefs.SetInt("Score", Score);
		}

	}

}