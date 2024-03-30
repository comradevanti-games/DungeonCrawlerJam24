using TMPro;
using UnityEngine;

namespace DGJ24.Menu.UI {

	internal class StatsDisplay : MonoBehaviour {

		[SerializeField]
		private TextMeshProUGUI scoreTextMesh = null!;

		private void Start() {

			if (!PlayerPrefs.HasKey("Score")) return;

			scoreTextMesh.gameObject.SetActive(true);
			scoreTextMesh.SetText("Highest Score: " + PlayerPrefs.GetInt("Score"));

		}

	}

}