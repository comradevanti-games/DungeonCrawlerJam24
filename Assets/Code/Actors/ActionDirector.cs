using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DGJ24.Tools;
using UnityEngine;

namespace DGJ24.Actors
{
    public class ActionDirector : MonoBehaviour
    {
        private HashSet<GameObject> ActivityPool { get; } = new();
        private Queue<IActionMonitor.ActionBatchReadyEvent> ActionBatchQueue { get; } = new();

        private void Awake()
        {
            Singletons.Get<IActionMonitor>().ActionBatchReady += TryExecute;
        }

        private void Update()
        {
            if (!ActivityPool.Any() && ActionBatchQueue.Count == 0)
                return;

            if (ActionBatchQueue.Count > 0)
            {
                TryExecute(ActionBatchQueue.Dequeue());
            }
        }

        private void TryExecute(IActionMonitor.ActionBatchReadyEvent batch)
        {
            if (ActivityPool.Count > 0)
            {
                ActionBatchQueue.Enqueue(batch);
                return;
            }

            foreach (ActionRequest? action in batch.Batch)
            {
                ActivityPool.Add(action.Actor);

                switch (action)
                {
                    case MovementActionRequest request:
                        MoveActor(request.Actor, request.Direction, request.MoveDuration);
                        break;
                    case RotationActionRequest request:
                        RotateActor(request.Actor, request.Rotation, request.RotateDuration);
                        break;
                    case TorchActionRequest request:
                        UseTorch(request.Actor, OnActionRequestExecuted);
                        break;
                    case InteractionActionRequest request:
                        Interact(request.Actor, request.TilePositions);
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

        private void MoveActor(GameObject actor, Direction direction, float duration)
        {
            Vector3 actorPosition = actor.transform.position;

            switch (direction)
            {
                case Direction.Forward:
                    StartCoroutine(
                        LerpPosition(
                            actor,
                            actorPosition,
                            actorPosition + (actor.transform.forward * 2f),
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    break;
                case Direction.Backward:
                    StartCoroutine(
                        LerpPosition(
                            actor,
                            actorPosition,
                            actorPosition + (actor.transform.forward * -2f),
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    break;
                case Direction.Left:
                    StartCoroutine(
                        LerpPosition(
                            actor,
                            actorPosition,
                            actorPosition + (actor.transform.right * -2f),
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    break;
                case Direction.Right:
                    StartCoroutine(
                        LerpPosition(
                            actor,
                            actorPosition,
                            actorPosition + (actor.transform.right * 2f),
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private void RotateActor(GameObject actor, Rotation rotation, float duration)
        {
            Quaternion origin = actor.transform.rotation;

            switch (rotation)
            {
                case Rotation.Right:
                {
                    Quaternion targetRotation =
                        actor.transform.rotation * Quaternion.Euler(0, 90, 0);
                    StartCoroutine(
                        LerpRotation(
                            actor,
                            origin,
                            targetRotation,
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    return;
                }
                case Rotation.Left:
                {
                    Quaternion targetRotation =
                        actor.transform.rotation * Quaternion.Euler(0, -90, 0);
                    StartCoroutine(
                        LerpRotation(
                            actor,
                            origin,
                            targetRotation,
                            duration,
                            OnActionRequestExecuted
                        )
                    );
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        private void UseTorch(GameObject actor, Action<GameObject> callback)
        {
            actor.GetComponentInChildren<Torch>().Flash();
            callback.Invoke(actor);
        }

        private void Interact(GameObject actor, Vector2Int[] interactedTiles)
        {
            GameObject? tileObject = FindObjectsByType<TileTransform>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None
                )
                .FirstOrDefault(tileTransform =>
                    interactedTiles.Any(x => tileTransform.Position == x)
                )
                ?.gameObject;

            // TODO: Interact with other Objects based on what they are.
            OnActionRequestExecuted(actor);
        }

        private void OnActionRequestExecuted(GameObject actor)
        {
            if (ActivityPool.Contains(actor))
            {
                ActivityPool.Remove(actor);
            }
        }

        private static IEnumerator LerpPosition(
            GameObject actor,
            Vector3 origin,
            Vector3 targetPosition,
            float duration,
            Action<GameObject> callback
        )
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                actor.transform.position = Vector3.Lerp(origin, targetPosition, progress);
                yield return null;
            }

            actor.transform.position = targetPosition;
            callback.Invoke(actor);
        }

        private static IEnumerator LerpRotation(
            GameObject actor,
            Quaternion origin,
            Quaternion targetRotation,
            float duration,
            Action<GameObject> callback
        )
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                actor.transform.rotation = Quaternion.Lerp(origin, targetRotation, progress);
                yield return null;
            }

            actor.transform.rotation = targetRotation;
            callback.Invoke(actor);
        }
    }
}
