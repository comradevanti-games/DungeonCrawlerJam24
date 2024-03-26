using UnityEngine;

namespace DGJ24.UI {

	public class HealthDisplay : MonoBehaviour {

		[SerializeField] private GameObject[]? healthPieces;

		public void OnPlayerHealthChanged(int newHealth) {

			for (int i = 0; i < healthPieces?.Length; i++) {
				healthPieces[i].SetActive(i < newHealth);
			}

		}

	}

}