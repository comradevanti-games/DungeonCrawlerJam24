using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal interface INavigationTarget
    {
        public Vector2Int Tile { get; }
    }

    public record TransformTarget(ITileTransform Target) : INavigationTarget
    {
        public Vector2Int Tile => Target.Position;
    }

    public record ConstantTarget(Vector2Int Tile) : INavigationTarget;
}
