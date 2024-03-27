using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.UI {

	internal class MenuOptionController : MonoBehaviour {

		public void Play() {
			SceneManager.LoadScene("Main");
		}

		public void Quit() {
			Application.Quit();
		}

	}

}