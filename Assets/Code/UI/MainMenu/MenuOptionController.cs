using UnityEngine;
using UnityEngine.SceneManagement;

namespace DGJ24.UI {

	public class MenuOptionController : MonoBehaviour {

		public void Play() {
			SceneManager.LoadScene("Main");
		}

		public void Quit() {
			Application.Quit();
		}

	}

}