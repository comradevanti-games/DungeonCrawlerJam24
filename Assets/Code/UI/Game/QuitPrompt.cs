using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DGJ24.UI {

	internal class QuitPrompt : MonoBehaviour {

		[SerializeField] private GameObject quitPanel;
		[SerializeField] private Image quitLoadingBar = null!;

		private Coroutine? holdRoutine;

		public void OnQuitHoldInitialized(double maxHoldDuration) {
			quitPanel.SetActive(true);
			holdRoutine = StartCoroutine(LerpBar(quitLoadingBar.fillAmount, 1, (float)maxHoldDuration));
		}

		public void OnQuitHoldCanceled() {
			quitPanel.SetActive(false);
			quitLoadingBar.fillAmount = 0;
			holdRoutine = null;
		}

		private IEnumerator LerpBar(float origin, float target, float duration) {

			float startTime = Time.time;
			float endTime = startTime + duration;

			while (Time.time < endTime) {
				float progress = (Time.time - startTime) / duration;
				quitLoadingBar.fillAmount = Mathf.Lerp(origin, target, progress);
				yield return null;
			}

			quitLoadingBar.fillAmount = target;

		}

	}

}