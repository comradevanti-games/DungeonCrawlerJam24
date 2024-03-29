using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Actors
{
    public abstract record ActionRequest(GameObject Actor);

    /// <summary>
    /// An action which does nothing.
    /// </summary>
    public record NoOpActionRequest(GameObject Actor) : ActionRequest(Actor);

    /// <param name="Direction">The direction to move in GLOBAL space.</param>
    public record MovementActionRequest(GameObject Actor, CardinalDirection Direction)
        : ActionRequest(Actor);

    public record RotationActionRequest(GameObject Actor, RotationDirection Rotation)
        : ActionRequest(Actor);

    public record InteractionActionRequest(GameObject Actor) : ActionRequest(Actor);

    public record ToolActionRequest(GameObject Actor) : ActionRequest(Actor);
}
