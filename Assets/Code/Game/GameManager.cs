using DGJ24.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.Game
{
    internal class GameManager : MonoBehaviour
    {
        private void HandleGameOver()
        {
            var score = Singletons.Get<IScoreTracker>().Score;
            PlayerPrefs.SetInt("Score", score);
			SceneManager.LoadScene("Menu");
        }

        public void OnPlayerDied()
        {
            HandleGameOver();
        }
    }
}
