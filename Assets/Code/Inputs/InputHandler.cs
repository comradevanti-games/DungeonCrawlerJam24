using UnityEngine;
using UnityEngine.InputSystem;

namespace DCJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				// Prevent Diagonal Inputs
				if (IsDiagonalInput(ctx.ReadValue<Vector2>()))
					return;

				Debug.Log(ctx.ReadValue<Vector2>());

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {
				Debug.Log(ctx.ReadValue<float>());
			}

		}

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

	}

}