using System;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;
using static DGJ24.TileSpace.TileSpaceMath;

namespace DGJ24.Actors {

	internal class ActionValidator : MonoBehaviour, IActionValidator {

		private IWalkableProvider walkableProvider = null!;

		private bool CanDoMove(MovementActionRequest request) {

			ITileTransform actorTransform = request.Actor.RequireComponent<ITileTransform>();
			Vector2Int destinationTile = MoveByDirection(actorTransform.Position, request.Direction);
			return walkableProvider.IsWalkable(destinationTile);

		}

		private bool CanUseTool(ToolActionRequest request) {

			IActorTool? usedTool = request.Actor.GetComponentInChildren<IActorTool>();
			return usedTool.RemainingCooldown <= 0;

		}

		public bool IsActionValid(ActionRequest request) {

			return request switch {
				NoOpActionRequest => true,
				RotationActionRequest => true,
				MovementActionRequest move => CanDoMove(move),
				// TODO: Check if interactable is in front of actor
				InteractionActionRequest => true,
				// TODO: Check charge
				ToolActionRequest toolUse => CanUseTool(toolUse),
				_ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
			};

		}

		private void Awake() {
			walkableProvider = Singletons.Get<IWalkableProvider>();
		}

	}

}