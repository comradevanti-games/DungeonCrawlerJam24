using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.Environment {

	internal class EmergencyLight : MonoBehaviour {

		[SerializeField] private Light light;
		[SerializeField] private float baseIntensity = 25f;
		[SerializeField] private float minIntensity = 0.5f;
		[SerializeField] private float maxIntensity = 1f;

		[SerializeField] private float[] flickerIntervals = Array.Empty<float>();

		public float minFlickerInterval = 0.5f;
		public float maxFlickerInterval = 10f;
		private float timeUntilNextFlicker;

		private bool isRunning = false;

		private void Start() {
			timeUntilNextFlicker = Random.Range(minFlickerInterval, maxFlickerInterval);
			StartCoroutine(Flicker());
		}

		private IEnumerator Flicker() {

			isRunning = true;

			while (isRunning) {

				timeUntilNextFlicker -= Time.deltaTime;

				if (timeUntilNextFlicker <= 0) {

					foreach (float f in flickerIntervals) {
						light.intensity = Random.Range(minIntensity, maxIntensity);
						yield return new WaitForSeconds(f);
					}

					light.intensity = baseIntensity;
					timeUntilNextFlicker = Random.Range(minFlickerInterval, maxFlickerInterval);

				}

				yield return null;

			}

		}

	}

}