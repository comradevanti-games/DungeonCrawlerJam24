using DGJ24.Score;
using TMPro;
using UnityEngine;

namespace DGJ24.UI {

	public class LootDisplay : MonoBehaviour {

		[SerializeField] private TextMeshProUGUI lootTextMesh = null!;

		private IScoreTracker? ScoreTracker { get; set; }

		private void Awake() {
			ScoreTracker = Singletons.Get<IScoreTracker>();
			ScoreTracker.ScoreUpdated += OnScoreUpdated;
		}

		private void OnScoreUpdated(int amount) {
			lootTextMesh.SetText($"x {amount}");
		}

	}

}