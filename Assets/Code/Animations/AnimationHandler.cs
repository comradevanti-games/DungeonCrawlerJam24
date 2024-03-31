using UnityEngine;

namespace DGJ24.Animations {

	[RequireComponent(typeof(Animator))]
	public class AnimationHandler : MonoBehaviour {

		private Animator AnimationController { get; set; } = null!;

		private void Awake() {
			AnimationController = GetComponent<Animator>();
		}

		public void Activate(string boolName) {
			AnimationController.SetBool(boolName, true);
		}

		public void Deactivate(string boolName) {
			AnimationController.SetBool(boolName, false);
		}

	}

}