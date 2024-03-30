using System;
using System.Collections;
using DGJ24.Interactables;
using DGJ24.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.Game
{
    internal class GameManager : MonoBehaviour
    {
        public void OnPlayerDied()
        {
            StartCoroutine(HandleGameOver());
        }

        public void OnExitSpawned(ExitSpawner.ExitSpawnedArgs args)
        {
            var exitInteractable = args.Exit.RequireComponent<IInteractable>();
            exitInteractable.Interacted += (_) => OnExited();
        }

        private void OnExited()
        {
            Debug.Log("Yay exit!");
            StartCoroutine(HandleGameOver());
        }

        private IEnumerator HandleGameOver()
        {
            var score = Singletons.Get<IScoreTracker>().Score;
            PlayerPrefs.SetInt("Score", score);
            yield return new WaitForSeconds(0.15f);
            SceneManager.LoadScene("Menu");
        }
    }
}
