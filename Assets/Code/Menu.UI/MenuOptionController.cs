using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.Menu.UI {

	internal class MenuOptionController : MonoBehaviour {

		[SerializeField] private GameObject tutorialDisplay = null!;
		[SerializeField] private GameObject controlsDisplay = null!;

		public void Play() {
			SceneManager.LoadScene("Main");
		}

		public void Quit() {
			Application.Quit();
		}

		public void ToggleTutorial() {

			if (controlsDisplay.activeSelf) {
				ToggleControls();
			}

			tutorialDisplay.SetActive(!tutorialDisplay.activeSelf);
		}

		public void ToggleControls() {

			if (tutorialDisplay.activeSelf) {
				ToggleTutorial();
			}

			controlsDisplay.SetActive(!controlsDisplay.activeSelf);

			if (controlsDisplay.activeSelf) {
				StartCoroutine(UnfoldKeyBindings());
			}
			else {
				foreach (Transform t in controlsDisplay.GetComponentsInChildren<Transform>()) {
					t.gameObject.SetActive(false);
				}
			}

		}

		private IEnumerator UnfoldKeyBindings() {
			
			foreach (RectTransform t in controlsDisplay.GetComponentsInChildren<RectTransform>(true)) {
				t.gameObject.SetActive(true);
				yield return new WaitForSeconds(0.005f);
			}

		}

	}

}