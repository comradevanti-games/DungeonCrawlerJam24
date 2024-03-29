using System.Collections;
using UnityEngine;

namespace DGJ24.Actors {

	internal class Torch : MonoBehaviour, IActorTool {

		[SerializeField] private Light? torchLight;
		[SerializeField] private AnimationCurve? flashCurve;
		[SerializeField] private float maxIntensity;
		[SerializeField] private float maxRange;
		[SerializeField] private float flashDuration;
		[SerializeField] private int cooldown;

		private AudioSource torchAudio = null!;

		private float baseIntensity;
		private float baseRange;
		private int lastRoundUsed;

		private bool isFlashing;

		public int Cooldown => cooldown;
		public int RemainingCooldown { get; private set; }

		private IActionDirector ActionDirector { get; set; }

		private void Awake() {
			baseIntensity = torchLight!.intensity;
			baseRange = torchLight.range;
			torchAudio = gameObject.RequireComponent<AudioSource>();
			ActionDirector = Singletons.Get<IActionDirector>();
			ActionDirector.AllActionsExecuted += OnRoundPassed;
		}

		public void Use() {

			if (isFlashing) return;

			StartCoroutine(Flashing(torchLight!, torchLight!.intensity, maxIntensity, torchLight.range, maxRange, flashDuration, flashCurve!));
			torchAudio.PlayOneShot(torchAudio.clip);

			RemainingCooldown = Cooldown + 1;

		}

		private void OnRoundPassed() {
			RemainingCooldown--;
		}

		private IEnumerator Flashing(Light l, float intensity, float intensityTarget, float range, float rangeTarget, float duration,
			AnimationCurve curve) {

			isFlashing = true;
			float current = 0f;

			while (current <= duration) {

				current += Time.deltaTime;
				float currentPercentage = Mathf.Clamp01(current / duration);
				float curvePercentage = curve.Evaluate(currentPercentage);

				l.intensity = Mathf.LerpUnclamped(intensity, intensityTarget, curvePercentage);
				l.range = Mathf.LerpUnclamped(range, rangeTarget, currentPercentage);
				yield return null;

			}

			l.intensity = baseIntensity;
			l.range = baseRange;

			isFlashing = false;

		}

	}

}