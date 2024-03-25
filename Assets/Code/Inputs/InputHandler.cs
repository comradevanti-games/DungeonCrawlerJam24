using DGJ24.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		[SerializeField] private float playerMoveDuration = 5f;
		[SerializeField] private float playerRotateDuration = 0.1f;

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
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Right, playerMoveDuration));
				}

				if (input.x < -0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Left, playerMoveDuration));
				}

				if (input.y < -0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Backward, playerMoveDuration));
				}

				if (input.y > 0.9) {
					ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, Direction.Forward, playerMoveDuration));
				}

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				if (ActionQueue == null) return;

				if (ctx.ReadValue<float>() > 0) {
					ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Right, playerRotateDuration));
				}

				if (ctx.ReadValue<float>() < 0) {
					ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Left, playerRotateDuration));
				}

			}

		}

		public void OnInteractionInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				ActionQueue.TryEnqueue(new InteractionActionRequest(gameObject));

			}

		}

		public void OnTorchInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				ActionQueue.TryEnqueue(new TorchActionRequest(gameObject));

			}

		}

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

	}

}