using System;
using DGJ24.Actors;
using DGJ24.Map;
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

		[SerializeField] private float playerMoveDuration = 5f;
		[SerializeField] private float playerRotateDuration = 0.1f;

		private IActionRequestQueue? ActionQueue { get; set; }
		private ITileTransform? TileTransform { get; set; }
		private IWalkableService? WalkableService { get; set; }

		private void Awake() {
			ActionQueue = GetComponent<IActionRequestQueue>();
			TileTransform = GetComponent<ITileTransform>();
			WalkableService = Singletons.Get<IWalkableService>();
			TileTransform.Position = Vector2Int.zero;
			TileTransform.Forward = CardinalDirection.Forward;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				Vector2 input = ctx.ReadValue<Vector2>();

				if (IsDiagonalInput(input)) return;

				if (TileTransform == null) return;

				if (WalkableService == null) return;

				if (ActionQueue == null) return;

				CardinalDirection inputDirection = GetInputDirection(input);

				Vector2Int destination =
					TileSpaceMath.GetDestinationTile(TileTransform.Position, TileTransform.LocalToGlobal(inputDirection) );

				if (!WalkableService.IsWalkable(destination)) {
					return;
				}

				if (ActionQueue.TryEnqueue(new MovementActionRequest(gameObject, inputDirection, playerMoveDuration))) {
					TileTransform.Position = destination;
				}

			}

		}

		public void OnRotationInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				if (TileTransform == null) return;

				if (ActionQueue == null) return;

				if (ctx.ReadValue<float>() > 0) {

					if (ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, RotationDirection.Right, playerRotateDuration))) {
						TileTransform.Forward = GetFacingDirection(RotationDirection.Right);
						playerRotationPerformed?.Invoke();
					}

				}

				if (ctx.ReadValue<float>() < 0) {

					if (ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, RotationDirection.Left, playerRotateDuration))) {
						TileTransform.Forward = GetFacingDirection(RotationDirection.Left);
						playerRotationPerformed?.Invoke();
					}

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

		private CardinalDirection GetFacingDirection(RotationDirection rotationDir) {

			if (rotationDir == RotationDirection.Left) {

				return TileTransform!.Forward switch {
					CardinalDirection.Right => CardinalDirection.Forward,
					CardinalDirection.Left => CardinalDirection.Backward,
					CardinalDirection.Forward => CardinalDirection.Left,
					CardinalDirection.Backward => CardinalDirection.Right,
					_ => throw new ArgumentOutOfRangeException()
				};

			}

			return TileTransform!.Forward switch {
				CardinalDirection.Right => CardinalDirection.Backward,
				CardinalDirection.Left => CardinalDirection.Forward,
				CardinalDirection.Forward => CardinalDirection.Right,
				CardinalDirection.Backward => CardinalDirection.Left,
				_ => throw new ArgumentOutOfRangeException()
			};

		}

		public void OnInteractionInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				if (TileTransform == null) return;

				Vector2Int interactionTile =
					TileSpaceMath.GetDestinationTile(TileTransform.Position, TileTransform.Forward);
				ActionQueue.TryEnqueue(new InteractionActionRequest(gameObject, interactionTile));

			}

		}

		public void OnTorchInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				ActionQueue.TryEnqueue(new ToolActionRequest(gameObject));

			}

		}

		public void OnQuitInput(InputAction.CallbackContext ctx) {
			
			HoldInteraction? holdInteraction = ctx.interaction as HoldInteraction;

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

		private bool IsDiagonalInput(Vector2 input) {
			return input.x != 0 && input.y != 0;
		}

	}

}