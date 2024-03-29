using System.Collections;
using DGJ24.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.Game {

	internal class GameManager : MonoBehaviour {

		public void OnPlayerDied() {
			var score = Singletons.Get<IScoreTracker>().Score;
			PlayerPrefs.SetInt("Score", score);
			StartCoroutine(HandleGameOver());
		}

		IEnumerator HandleGameOver() {

			yield return new WaitForSeconds(0.15f);
			SceneManager.LoadScene("Menu");

		}

	}

}