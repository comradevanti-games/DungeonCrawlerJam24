using TMPro;
using UnityEngine;

namespace DGJ24.Menu.UI {

	internal class StatsDisplay : MonoBehaviour {

		[SerializeField] private GameObject scoreObject = null!;
		[SerializeField] private TextMeshProUGUI scoreTextMesh = null!;
		[SerializeField] private GameObject exitScoreSymbol = null!;

		private void Start() {

			if (!(PlayerPrefs.HasKey("Score") || PlayerPrefs.HasKey("ExitScore"))) return;

			scoreObject.gameObject.SetActive(true);

			if (PlayerPrefs.HasKey("ExitScore")) {
				exitScoreSymbol.SetActive(true);
				scoreTextMesh.SetText("Highest Score: " + PlayerPrefs.GetInt("ExitScore"));
			}
			else {
				scoreTextMesh.SetText("Highest Score: " + PlayerPrefs.GetInt("Score"));
			}

		}

	}

}