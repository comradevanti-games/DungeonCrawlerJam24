using DGJ24.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		private ActionRequestQueue? ActionQueue { get; set; }

		private void Awake() {
			ActionQueue = GetComponent<ActionRequestQueue>();
		}

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				Vector2 input = ctx.ReadValue<Vector2>();

				// Prevent Diagonal Inputs
				if (IsDiagonalInput(input)) return;

				if (ActionQueue == null) return;

				if (input.x > 0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Right));
				}

				if (input.x < -0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Left));
				}

				if (input.y < -0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Backward));
				}

				if (input.y > 0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Forward));
				}

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				if (ActionQueue == null) return;

				if (ctx.ReadValue<float>() > 0) {
					ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Right));
				}

				if (ctx.ReadValue<float>() < 0) {
					ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Left));
				}

			}

		}

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

		private Direction GetMovementDirection() {
			return Direction.Forward;
		}

	}

}