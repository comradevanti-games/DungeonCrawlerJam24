using DGJ24.Actors;
using DGJ24.TileSpace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

namespace DGJ24.Inputs {

	internal class InputHandler : MonoBehaviour {

		public UnityEvent? playerRotationPerformed;
		public UnityEvent<double>? quitInputInitialized;
		public UnityEvent? quitInputCanceled;

		[SerializeField]
		private float playerMoveDuration = 5f;

		[SerializeField]
		private float playerRotateDuration = 0.1f;

		private IActionRequestQueue ActionQueue { get; set; } = null!;
		private ITileTransform TileTransform { get; set; } = null!;

		private void Awake() {
			ActionQueue = gameObject.RequireComponent<IActionRequestQueue>();
			TileTransform = gameObject.RequireComponent<ITileTransform>();
			TileTransform.Position = Vector2Int.zero;
			TileTransform.Forward = CardinalDirection.Forward;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void OnMovementInput(InputAction.CallbackContext ctx) {
			if (ctx.performed) {
				Vector2 input = ctx.ReadValue<Vector2>();

				if (IsDiagonalInput(input))
					return;

				CardinalDirection localDir = GetInputDirection(input);
				var globalDir = TileTransform.LocalToGlobal(localDir);

				_ = ActionQueue.TryEnqueue(
					new MovementActionRequest(gameObject, globalDir, playerMoveDuration)
				);
			}
		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {
			if (ctx.performed) {
				var input = ctx.ReadValue<float>();
				if (input == 0)
					return;
				var turnDir = input > 0 ? RotationDirection.Right : RotationDirection.Left;

				if (
					ActionQueue.TryEnqueue(
						new RotationActionRequest(gameObject, turnDir, playerRotateDuration)
					)
				) {
					playerRotationPerformed?.Invoke();
				}
			}
		}

		private CardinalDirection GetInputDirection(Vector2 input) {
			switch (input.x) {
				case > 0.9f:
					return CardinalDirection.Right;
				case < -0.9f:
					return CardinalDirection.Left;
			}

			switch (input.y) {
				case > 0.9f:
					return CardinalDirection.Forward;
				case < -0.9f:
					return CardinalDirection.Backward;
			}

			return CardinalDirection.Forward;
		}

		public void OnInteractionInput(InputAction.CallbackContext ctx) {
			if (ctx.canceled) {
				ActionQueue.TryEnqueue(new InteractionActionRequest(gameObject));
			}
		}

		public void OnTorchInput(InputAction.CallbackContext ctx) {
			if (ctx.canceled) {
				ActionQueue.TryEnqueue(new ToolActionRequest(gameObject));
			}
		}

		public void OnQuitInput(InputAction.CallbackContext ctx) {
			HoldInteraction holdInteraction = (ctx.interaction as HoldInteraction)!;

			if (ctx.started) {
				quitInputInitialized?.Invoke(holdInteraction.duration);
			}

			if (ctx.performed) {
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				SceneManager.LoadScene("Menu");
			}

			if (ctx.canceled) {
				quitInputCanceled?.Invoke();
			}
		}

		public void ReleaseCursor() {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

	}

}