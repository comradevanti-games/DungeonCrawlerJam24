using System;
using System.Collections.Generic;
using System.Linq;
using DGJ24.TileSpace;
using DGJ24.Tools;
using UnityEngine;

namespace DGJ24.Actors {

	internal class ActionDirector : MonoBehaviour, IActorDirector {

		public event Action? AllActionsExecuted;

		private ITileSpaceEntityRepo tileSpaceEntityRepo = null!;
		private HashSet<GameObject> ActivityPool { get; } = new();

		private void Awake() {
			tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
			Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
		}

		private void TryExecute(IActionMonitor.ActionBatchReadyEvent batch) {
			ActivityPool.UnionWith(batch.Batch.Select(it => it.Actor));

			foreach (ActionRequest? action in batch.Batch) {
				switch (action) {
					case MovementActionRequest request:
						MoveActor(request.Actor, request.Direction, request.MoveDuration);
						break;
					case RotationActionRequest request:
						RotateActor(request.Actor, request.Rotation, request.RotateDuration);
						break;
					case ToolActionRequest request:
						UseTool(request.Actor, OnActionRequestExecuted);
						break;
					case InteractionActionRequest request:
						Interact(request.Actor);
						break;
					case NoOpActionRequest:
						OnActionRequestExecuted(action.Actor);
						break;
					default:
						throw new ArgumentException(
							$"Unhandled action-request type: {action.GetType().Name}"
						);
				}
			}
		}

		private void MoveActor(GameObject actor, CardinalDirection direction, float duration) {
			var actorTransform = actor.RequireComponent<ITileTransform>();

			var actorTile = actorTransform.Position;
			var nextTile = TileSpaceMath.MoveByDirection(actorTile, direction);
			actorTransform.Position = nextTile;

			Vector3 actorPosition = TileSpaceMath.PositionToWorldSpace(actorTile);
			Vector3 targetPosition = TileSpaceMath.PositionToWorldSpace(nextTile);

			StartCoroutine(
				LerpOverTime.Position(
					actor.transform,
					actorPosition,
					targetPosition,
					duration,
					() => OnActionRequestExecuted(actor)
				)
			);
		}

		private void RotateActor(GameObject actor, RotationDirection rotation, float duration) {
			var actorTransform = actor.RequireComponent<ITileTransform>();
			var actorForward = actorTransform.Forward;
			actorTransform.Rotate(rotation);
			var nextForward = actorTransform.Forward;

			Quaternion origin = Quaternion.LookRotation(
				TileSpaceMath.DirectionToWorldSpace(actorForward)
			);
			Quaternion targetRotation = Quaternion.LookRotation(
				TileSpaceMath.DirectionToWorldSpace(nextForward)
			);

			StartCoroutine(
				LerpOverTime.Rotation(
					actor.transform,
					origin,
					targetRotation,
					duration,
					() => OnActionRequestExecuted(actor)
				)
			);
		}

		private void UseTool(GameObject actor, Action<GameObject> callback) {
			actor.GetComponentInChildren<ITool>().Use();
			callback.Invoke(actor);
		}

		private void Interact(GameObject actor) {
			/*
			IEnumerable<GameObject> entities = tileSpaceEntityRepo.All.Where(entity =>
			    interactedTiles.Any(x => entity.Position == x)
			);
			*/

			// TODO: Interact with other Objects based on what they are.
			OnActionRequestExecuted(actor);
		}

		private void OnActionRequestExecuted(GameObject actor) {
			if (ActivityPool.Contains(actor)) {
				ActivityPool.Remove(actor);

				if (ActivityPool.Count == 0) {
					AllActionsExecuted?.Invoke();
				}
			}
		}

	}

}