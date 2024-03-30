using System;
using System.Collections;
using DGJ24.Interactables;
using DGJ24.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.Game {

	internal class GameManager : MonoBehaviour {

		public void OnPlayerDied() {
			StartCoroutine(HandleGameOver(false));
		}

		public void OnExitSpawned(ExitSpawner.ExitSpawnedArgs args) {
			var exitInteractable = args.Exit.RequireComponent<IInteractable>();
			exitInteractable.Interacted += (_) => OnExited();
		}

		private void OnExited() {
			StartCoroutine(HandleGameOver(true));
		}

		private IEnumerator HandleGameOver(bool hasExited) {
			var score = Singletons.Get<IScoreTracker>().Score;

			if (score > 0) {
				PlayerPrefs.SetInt(hasExited ? "ExitScore" : "Score", score);
			}
			
			yield return new WaitForSeconds(0.25f);
			SceneManager.LoadScene("Menu");
		}

	}

}