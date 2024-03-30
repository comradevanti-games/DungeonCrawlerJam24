using System;
using System.Collections.Generic;
using System.Linq;
using DGJ24.Interactables;
using DGJ24.Movement;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Actors {

	internal class ActionDirector : MonoBehaviour, IActionDirector {

		public event Action? AllActionsExecuted;
		
		private HashSet<GameObject> ActivityPool { get; } = new();
		private int TotalRoundCount { get; set; }

		private void Awake() {
			Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
		}

		private void TryExecute(IActionMonitor.ActionBatchReadyArgs batch) {
			ActivityPool.UnionWith(batch.Batch.Select(it => it.Actor));

			foreach (ActionRequest? action in batch.Batch) {
				switch (action) {
					case MovementActionRequest request:
						MoveActor(request.Actor, request.Direction);
						break;
					case RotationActionRequest request:
						RotateActor(request.Actor, request.Rotation);
						break;
					case ToolActionRequest request:
						UseTool(request.Actor, OnActionRequestExecuted);
						break;
					case InteractionActionRequest request:
						TryInteract(request.Actor);
						OnActionRequestExecuted(request.Actor);
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

		private async void MoveActor(GameObject actor, CardinalDirection direction) {
			var actorTransform = actor.RequireComponent<ITileTransform>();
			actorTransform.MoveIn(direction);

			var transformAnimator = actor.RequireComponent<ITransformAnimator>();
			await transformAnimator.SyncPosition();
			
			if(destroyCancellationToken.IsCancellationRequested) return;
			OnActionRequestExecuted(actor);
		}

		private async void RotateActor(GameObject actor, RotationDirection rotation) {
			var actorTransform = actor.RequireComponent<ITileTransform>();
			actorTransform.Rotate(rotation);

			var transformAnimator = actor.RequireComponent<ITransformAnimator>();
			await transformAnimator.SyncRotation();
			
			if(destroyCancellationToken.IsCancellationRequested) return;
			OnActionRequestExecuted(actor);
		}

		private void UseTool(GameObject actor, Action<GameObject> callback) {
			actor.GetComponentInChildren<IActorTool>().Use();
			callback.Invoke(actor);
		}

		private void TryInteract(GameObject actor) {
			var interactor = actor.GetComponent<IInteractor>();
			interactor?.TryInteract();
		}

		private void OnActionRequestExecuted(GameObject actor) {
			if (ActivityPool.Contains(actor)) {
				ActivityPool.Remove(actor);

				if (ActivityPool.Count == 0) {
					AllActionsExecuted?.Invoke();
					TotalRoundCount++;
				}
			}
		}

	}

}