using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.Audio {

	[RequireComponent(typeof(AudioSource))]
	public class AudioPlayer : MonoBehaviour {

		[SerializeField] private AudioSource source = null!;
		[SerializeField] private AudioClip[] availableAudioClips = Array.Empty<AudioClip>();

		private void Awake() {
			source = GetComponent<AudioSource>();
		}

		public void PlayAudioClip(string clipName) {
			source.PlayOneShot(availableAudioClips.FirstOrDefault(it => it.name == clipName));
		}

		public void PlayRandomAudioClip() {

			int r = Random.Range(0, availableAudioClips.Length);
			source.PlayOneShot(availableAudioClips[r]);

		}

	}

}