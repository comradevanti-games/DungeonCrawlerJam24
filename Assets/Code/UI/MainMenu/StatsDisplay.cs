using TMPro;
using UnityEngine;

namespace DGJ24.UI {

	public class StatsDisplay : MonoBehaviour {

		[SerializeField] private TextMeshProUGUI scoreTextMesh = null!;

		private void Start() {

			if (PlayerPrefs.HasKey("Score")) {
				scoreTextMesh.gameObject.SetActive(true);
				scoreTextMesh.SetText("HighScore: " + PlayerPrefs.GetInt("Score"));
			}

		}

	}

}