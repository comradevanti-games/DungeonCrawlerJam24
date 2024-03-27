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
			TileTransform.Forward = GridDirection.ZPlus;
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
					TileSpaceMath.GetDestinationTile(TileTransform.Position, GetRelativeDirection(inputDirection, TileTransform.Forward));

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

					if (ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Right, playerRotateDuration))) {
						TileTransform.Forward = GetFacingDirection(Rotation.Right);
						playerRotationPerformed?.Invoke();
					}

				}

				if (ctx.ReadValue<float>() < 0) {

					if (ActionQueue.TryEnqueue(new RotationActionRequest(gameObject, Rotation.Left, playerRotateDuration))) {
						TileTransform.Forward = GetFacingDirection(Rotation.Left);
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

		private GridDirection GetFacingDirection(Rotation rotationDir) {

			if (rotationDir == Rotation.Left) {

				return TileTransform!.Forward switch {
					GridDirection.XPlus => GridDirection.ZPlus,
					GridDirection.XMinus => GridDirection.ZMinus,
					GridDirection.ZPlus => GridDirection.XMinus,
					GridDirection.ZMinus => GridDirection.XPlus,
					_ => throw new ArgumentOutOfRangeException()
				};

			}

			return TileTransform!.Forward switch {
				GridDirection.XPlus => GridDirection.ZMinus,
				GridDirection.XMinus => GridDirection.ZPlus,
				GridDirection.ZPlus => GridDirection.XPlus,
				GridDirection.ZMinus => GridDirection.XMinus,
				_ => throw new ArgumentOutOfRangeException()
			};

		}

		private GridDirection GetRelativeDirection(CardinalDirection input, GridDirection currentForward) {

			return currentForward switch {
				GridDirection.XPlus => input switch {
					CardinalDirection.Forward => GridDirection.XPlus,
					CardinalDirection.Backward => GridDirection.XMinus,
					CardinalDirection.Left => GridDirection.ZPlus,
					CardinalDirection.Right => GridDirection.ZMinus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.XMinus => input switch {
					CardinalDirection.Forward => GridDirection.XMinus,
					CardinalDirection.Backward => GridDirection.XPlus,
					CardinalDirection.Left => GridDirection.ZMinus,
					CardinalDirection.Right => GridDirection.ZPlus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.ZPlus => input switch {
					CardinalDirection.Forward => GridDirection.ZPlus,
					CardinalDirection.Backward => GridDirection.ZMinus,
					CardinalDirection.Left => GridDirection.XMinus,
					CardinalDirection.Right => GridDirection.XPlus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.ZMinus => input switch {
					CardinalDirection.Forward => GridDirection.ZMinus,
					CardinalDirection.Backward => GridDirection.ZPlus,
					CardinalDirection.Left => GridDirection.XPlus,
					CardinalDirection.Right => GridDirection.XMinus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				_ => throw new ArgumentOutOfRangeException(nameof(currentForward), currentForward, null)
			};

		}

		public void OnInteractionInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				if (TileTransform == null) return;

				Vector2Int interactionTile =
					TileSpaceMath.GetDestinationTile(TileTransform.Position, GetRelativeDirection(CardinalDirection.Forward, TileTransform.Forward));
				ActionQueue.TryEnqueue(new InteractionActionRequest(gameObject, interactionTile));

			}

		}

		public void OnTorchInput(InputAction.CallbackContext ctx) {

			if (ctx.canceled) {

				if (ActionQueue == null) return;

				ActionQueue.TryEnqueue(new TorchActionRequest(gameObject));

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