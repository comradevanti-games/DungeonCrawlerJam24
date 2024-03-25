using UnityEngine;

namespace DGJ24.Actors {

	public abstract record ActionRequest(GameObject Actor);

	public record MovementActionRequest(GameObject Actor, Direction Direction) : ActionRequest(Actor);

	public record RotationActionRequest(GameObject Actor, Rotation Rotation) : ActionRequest(Actor);

	public record InteractionActionRequest(GameObject Actor) : ActionRequest(Actor);

	public record TorchActionRequest(GameObject Actor) : ActionRequest(Actor);

}