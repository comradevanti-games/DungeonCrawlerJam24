using UnityEngine;

namespace DGJ24.Game.UI {

	internal class HealthDisplay : MonoBehaviour {

		[SerializeField] private GameObject[]? healthPieces;

		public void OnPlayerHealthChanged(int newHealth) {

			for (int i = 0; i < healthPieces?.Length; i++) {
				healthPieces[i].SetActive(i < newHealth);
			}

		}

	}

}