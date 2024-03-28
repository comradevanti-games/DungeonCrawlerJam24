using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Shuffle : MonoBehaviour {

	[SerializeField] private AudioClip[] shuffleClips = Array.Empty<AudioClip>();
	[SerializeField] private float minInterval;
	[SerializeField] private float maxInterval;

	private AudioSource AudioSource { get; set; } = null!;

	private float interval = 15f;

	private void Awake() {
		AudioSource = GetComponent<AudioSource>();
		interval = Random.Range(minInterval, maxInterval);
	}

	private void Update() {

		interval -= Time.deltaTime;
		if (!(interval <= 0)) return;

		PlayRandomSfx();
		interval = Random.Range(minInterval, maxInterval);

	}

	private void PlayRandomSfx() {

		int choice = Random.Range(0, shuffleClips.Length);
		AudioSource.PlayOneShot(shuffleClips[choice]);

	}

}