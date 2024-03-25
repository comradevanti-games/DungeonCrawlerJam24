using UnityEngine;

namespace DGJ24.Actors {

	public abstract record ActionRequest(GameObject Actor);

	public record MovementActionRequest(GameObject Actor, Direction Direction, float MoveDuration) : ActionRequest(Actor);

	public record RotationActionRequest(GameObject Actor, Rotation Rotation, float RotateDuration) : ActionRequest(Actor);

	public record InteractionActionRequest(GameObject Actor, params Vector2Int[] TilePositions) : ActionRequest(Actor);

	public record TorchActionRequest(GameObject Actor) : ActionRequest(Actor);

}