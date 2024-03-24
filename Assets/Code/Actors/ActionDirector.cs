using UnityEngine;

namespace DGJ24.Actors {

	public class ActionDirector : MonoBehaviour {

		private void Awake() {
			Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
		}

		public static void TryExecute(IActionMonitor.ActionBatchReadyEvent batch) {
			// Check for conflicts

			foreach (ActionRequest? action in batch.Batch) {

				switch (action) {

					case MovementActionRequest move:

						if (move.Direction == Direction.Forward) {
							// Forward Movement
						}

						break;
				}

			}
		}

	}

}