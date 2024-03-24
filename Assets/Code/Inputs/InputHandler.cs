using System;
using System.Collections.Generic;
using DGJ24.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		private void Awake() {
			
		}

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				Vector2 input = ctx.ReadValue<Vector2>();

				// Prevent Diagonal Inputs
				if (IsDiagonalInput(input))
					return;

				Direction dir;

				if (input.x > 0.9) {
					dir = Direction.Right;
					MovementActionRequest mar = new(gameObject, dir);
				}

				if (input.x < -0.9) {
					dir = Direction.Left;
					MovementActionRequest mar = new(gameObject, dir);
				}

				if (input.y < -0.9) {
					dir = Direction.Back;
					MovementActionRequest mar = new(gameObject, dir);
				}

				if (input.y > 0.9) {
					dir = Direction.Forward;
					MovementActionRequest mar = new(gameObject, dir);
				}

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {
				//rotationInputCaptured?.Invoke(ctx.ReadValue<float>());
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