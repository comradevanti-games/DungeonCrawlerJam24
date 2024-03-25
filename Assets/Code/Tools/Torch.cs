using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DGJ24.Tools {

	public class Torch : MonoBehaviour {

		[SerializeField] private Light torchLight;
		[SerializeField] private AnimationCurve intensityCurve;
		[SerializeField] private AnimationCurve rangeCurve;
		[SerializeField] private float maxIntensity;
		[SerializeField] private float maxRange;

		private bool isFlashing;

		public void Flash() {

			if (isFlashing) return;
			
			Debug.Log("Flashed");

			isFlashing = true;
			StartCoroutine(LerpOnCurve(torchLight.intensity, maxIntensity, 0.5f, intensityCurve));

		}

		public IEnumerator LerpOnCurve(float t, float target, float duration, AnimationCurve curve) {

			float current = 0f;

			while (current <= duration) {

				current += Time.deltaTime;
				float currentPercentage = Mathf.Clamp01(current / duration);
				float curvePercentage = curve.Evaluate(currentPercentage);

				torchLight.intensity = Mathf.LerpUnclamped(t, target, curvePercentage);
				yield return null;

			}

			torchLight.intensity = 1;
			isFlashing = false;
			Debug.Log(isFlashing);

		}

	}

}