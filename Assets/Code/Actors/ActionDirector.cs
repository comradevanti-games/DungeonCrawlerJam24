using System;
using System.Collections;
using UnityEngine;

namespace DGJ24.Actors {

	public class ActionDirector : MonoBehaviour {

		private void Awake() {
			Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
		}

		private void TryExecute(IActionMonitor.ActionBatchReadyEvent batch) {

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

				case Direction.Forward:
					StartCoroutine(LerpPosition(actor, actorPosition, actorPosition + (actor.transform.forward * 2f), 0.2f));
					break;
				case Direction.Backward:
					StartCoroutine(LerpPosition(actor, actorPosition, actorPosition + (actor.transform.forward * -2f), 0.2f));
					break;
				case Direction.Left:
					StartCoroutine(LerpPosition(actor, actorPosition, actorPosition + (actor.transform.right * -2f), 0.2f));
					break;
				case Direction.Right:
					StartCoroutine(LerpPosition(actor, actorPosition, actorPosition + (actor.transform.right * 2f), 0.2f));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);

			}

		}

		private void RotateActor(GameObject actor, Rotation rotation) {

			switch (rotation) {

				case Rotation.Right: {
					Quaternion targetRotation = actor.transform.rotation * Quaternion.Euler(0, 90, 0);
					StartCoroutine(LerpRotation(actor, actor.transform.rotation, targetRotation, 0.1f));
					return;
				}
				case Rotation.Left: {
					Quaternion targetRotation = actor.transform.rotation * Quaternion.Euler(0, -90, 0);
					StartCoroutine(LerpRotation(actor, actor.transform.rotation, targetRotation, 0.1f));
					break;
				}

			}

		}

		private static IEnumerator LerpPosition(GameObject actor, Vector3 origin, Vector3 targetPosition, float duration) {

			float startTime = Time.time;
			float endTime = startTime + duration;

			while (Time.time < endTime) {
				float progress = (Time.time - startTime) / duration;
				actor.transform.position = Vector3.Lerp(origin, targetPosition, progress);
				yield return null;
			}

			actor.transform.position = targetPosition;

		}

		private static IEnumerator LerpRotation(GameObject actor, Quaternion origin, Quaternion targetRotation, float duration) {

			float startTime = Time.time;
			float endTime = startTime + duration;

			while (Time.time < endTime) {
				float progress = (Time.time - startTime) / duration;
				actor.transform.rotation = Quaternion.Lerp(origin, targetRotation, progress);
				yield return null;
			}

			actor.transform.rotation = targetRotation;

		}

	}

}