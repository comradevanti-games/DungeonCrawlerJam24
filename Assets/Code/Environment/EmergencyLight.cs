using System.Collections;
using UnityEngine;

namespace DGJ24.Environment {

	internal class EmergencyLight : MonoBehaviour {

		[SerializeField] private Light light;
		[SerializeField] private float baseIntensity = 25f;
		[SerializeField] private float minIntensity = 0.5f;
		[SerializeField] private float maxIntensity = 1f;

		public float minFlickerInterval = 0.5f;
		public float maxFlickerInterval = 10f;
		private float timeUntilNextFlicker;

		private bool isRunning = false;

		private void Start() {
			timeUntilNextFlicker = Random.Range(minFlickerInterval, maxFlickerInterval);
			StartCoroutine(Flicker());
		}

		IEnumerator Flicker() {

			isRunning = true;

			while (isRunning) {

				timeUntilNextFlicker -= Time.deltaTime;

				if (timeUntilNextFlicker <= 0) {

					light.intensity = Random.Range(minIntensity, maxIntensity);
					yield return new WaitForSeconds(0.2f);
					light.intensity = Random.Range(minIntensity, maxIntensity);
					yield return new WaitForSeconds(0.07f);
					light.intensity = Random.Range(minIntensity, maxIntensity);
					yield return new WaitForSeconds(0.145f);
					light.intensity = Random.Range(minIntensity, maxIntensity);
					yield return new WaitForSeconds(2f);

					light.intensity = baseIntensity;
					timeUntilNextFlicker = Random.Range(minFlickerInterval, maxFlickerInterval);

				}

				yield return null;

			}

		}

	}

}