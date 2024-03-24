using System;
using System.Collections;
using UnityEngine;

namespace DGJ24.Actors {

	public class ActionDirector : MonoBehaviour {

		private void Awake() {
			Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
		}

		private void TryExecute(IActionMonitor.ActionBatchReadyEvent batch) {
			// Check for conflicts

			foreach (ActionRequest? action in batch.Batch) {

				switch (action) {

					case MovementActionRequest request:
						MoveActor(request.Actor, request.Direction);
						break;
					case RotationActionRequest request:
						RotateActor(request.Actor, request.Rotation);
						break;

				}

			}

		}

		private void MoveActor(GameObject actor, Direction direction) {

			Vector3 actorPosition = actor.transform.position;

			switch (direction) {
				
				//TODO: Use Real Movement

				case Direction.Forward:
					StartCoroutine(LerpPosition(actor, actor.transform.position, new Vector3(actorPosition.x, actorPosition.y, actorPosition.z + 1),
						0.2f));
					break;
				case Direction.Backward:
					StartCoroutine(LerpPosition(actor, actor.transform.position, new Vector3(actorPosition.x, actorPosition.y, actorPosition.z - 1),
						0.2f));
					break;
				case Direction.Left:
					StartCoroutine(LerpPosition(actor, actor.transform.position, new Vector3(actorPosition.x - 1, actorPosition.y, actorPosition.z),
						0.2f));
					break;
				case Direction.Right:
					StartCoroutine(LerpPosition(actor, actor.transform.position, new Vector3(actorPosition.x + 1, actorPosition.y, actorPosition.z),
						0.2f));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);

			}

		}

		private static void RotateActor(GameObject actor, Rotation rotation) { }

		private static IEnumerator LerpPosition(GameObject actor, Vector3 origin, Vector3 targetPosition, float duration) {

			float startTime = Time.time;

			while (Time.time - startTime < duration) {
				float progress = (Time.time - startTime) / duration;
				actor.transform.position = Vector3.Lerp(origin, targetPosition, progress);
				yield return null;
			}

			actor.transform.position = targetPosition;

		}

	}

}