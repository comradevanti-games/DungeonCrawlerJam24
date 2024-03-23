using DGJ24.Inputs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DCJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		public UnityEvent<Direction>? movementInputCaptured;
		public UnityEvent<float>? rotationInputCaptured;

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				Vector2 input = ctx.ReadValue<Vector2>();

				// Prevent Diagonal Inputs
				if (IsDiagonalInput(input))
					return;

				Direction dir;

				if (input.x > 0.9) {
					dir = Direction.Right;
					movementInputCaptured?.Invoke(dir);
				}

				if (input.x < -0.9) {
					dir = Direction.Left;
					movementInputCaptured?.Invoke(dir);
				}

				if (input.y < -0.9) {
					dir = Direction.Back;
					movementInputCaptured?.Invoke(dir);
				}

				if (input.y > 0.9) {
					dir = Direction.Forward;
					movementInputCaptured?.Invoke(dir);
				}

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {
				rotationInputCaptured?.Invoke(ctx.ReadValue<float>());
			}

		}

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

	}

}