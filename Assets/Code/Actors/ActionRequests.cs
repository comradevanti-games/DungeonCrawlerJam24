using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Actors
{
    public abstract record ActionRequest(GameObject Actor);

    /// <summary>
    /// An action which does nothing.
    /// </summary>
    public record NoOpActionRequest(GameObject Actor) : ActionRequest(Actor);

    public record MovementActionRequest(GameObject Actor, CardinalDirection Direction, float MoveDuration)
        : ActionRequest(Actor);

    public record RotationActionRequest(GameObject Actor, RotationDirection Rotation, float RotateDuration)
        : ActionRequest(Actor);

    public record InteractionActionRequest(GameObject Actor, params Vector2Int[] TilePositions)
        : ActionRequest(Actor);

    public record ToolActionRequest(GameObject Actor) : ActionRequest(Actor);
}
