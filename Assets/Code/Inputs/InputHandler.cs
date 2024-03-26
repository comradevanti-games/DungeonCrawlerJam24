using System;
using DGJ24.Actors;
using DGJ24.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	public class InputHandler : MonoBehaviour {

		public UnityEvent? playerRotationPerformed;

		[SerializeField] private float playerMoveDuration = 5f;
		[SerializeField] private float playerRotateDuration = 0.1f;

		private ActionRequestQueue? ActionQueue { get; set; }
		private TileTransform? TileTransform { get; set; }
		private IWalkableService? WalkableService { get; set; }

		private void Awake() {
			ActionQueue = GetComponent<ActionRequestQueue>();
			TileTransform = GetComponent<TileTransform>();
			WalkableService = Singletons.Get<IWalkableService>();
			TileTransform.Position = Vector2Int.zero;
			TileTransform.Forward = GridDirection.ZPlus;
		}

		public void OnMovementInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				Vector2 input = ctx.ReadValue<Vector2>();

				if (IsDiagonalInput(input)) return;

				if (TileTransform == null) return;

				if (WalkableService == null) return;

				if (ActionQueue == null) return;

				Direction inputDirection = GetInputDirection(input);

				Vector2Int destination =
					TileSpace.GetDestinationTile(TileTransform.Position, GetRelativeDirection(inputDirection, TileTransform.Forward));

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

		private Direction GetInputDirection(Vector2 input) {

			switch (input.x) {
				case > 0.9f:
					return Direction.Right;
				case < -0.9f:
					return Direction.Left;
			}

			switch (input.y) {
				case > 0.9f:
					return Direction.Forward;
				case < -0.9f:
					return Direction.Backward;
			}

			return Direction.Forward;

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

		private GridDirection GetRelativeDirection(Direction input, GridDirection currentForward) {

			return currentForward switch {
				GridDirection.XPlus => input switch {
					Direction.Forward => GridDirection.XPlus,
					Direction.Backward => GridDirection.XMinus,
					Direction.Left => GridDirection.ZPlus,
					Direction.Right => GridDirection.ZMinus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.XMinus => input switch {
					Direction.Forward => GridDirection.XMinus,
					Direction.Backward => GridDirection.XPlus,
					Direction.Left => GridDirection.ZMinus,
					Direction.Right => GridDirection.ZPlus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.ZPlus => input switch {
					Direction.Forward => GridDirection.ZPlus,
					Direction.Backward => GridDirection.ZMinus,
					Direction.Left => GridDirection.XMinus,
					Direction.Right => GridDirection.XPlus,
					_ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
				},
				GridDirection.ZMinus => input switch {
					Direction.Forward => GridDirection.ZMinus,
					Direction.Backward => GridDirection.ZPlus,
					Direction.Left => GridDirection.XPlus,
					Direction.Right => GridDirection.XMinus,
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
					TileSpace.GetDestinationTile(TileTransform.Position, GetRelativeDirection(Direction.Forward, TileTransform.Forward));
				ActionQueue.TryEnqueue(new InteractionActionRequest(gameObject, interactionTile));

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