using System.Collections.Generic;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal interface INavigationTarget
    {
        public Vector2Int? Tile { get; }

        public void Update();
    }

    internal record TransformTarget(ITileTransform Target) : INavigationTarget
    {
        public Vector2Int? Tile { get; private set; }

        public void Update()
        {
            Tile = Target.Position;
        }
    }

    internal record ConstantTarget(Vector2Int? Tile) : INavigationTarget
    {
        public void Update() { }
    }

    internal class DelayedTarget : INavigationTarget
    {
        private readonly Queue<Vector2Int?> targetQueue;
        private readonly INavigationTarget inner;
        private readonly int delay;

        public Vector2Int? Tile => targetQueue.TryPeek(out var tile) ? tile : null;

        public DelayedTarget(INavigationTarget inner, int delay)
        {
            this.inner = inner;
            this.delay = delay;
            targetQueue = new Queue<Vector2Int?>(delay);
        }

        public void Update()
        {
            if (targetQueue.Count == delay)
                _ = targetQueue.Dequeue();

            inner.Update();

            var next = inner.Tile;
            targetQueue.Enqueue(next);
        }
    }

    internal record PredictiveTarget(
        ITileTransform Target,
        IFloorPlan FloorPlan,
        int PredictionDistance
    ) : INavigationTarget
    {
        public Vector2Int? Tile { get; private set; }

        public void Update()
        {
            var currentTile = Target.Position;
            var predicted =
                currentTile + TileSpaceMath.VectorForDirection(Target.Forward) * PredictionDistance;
            Tile = FloorPlan.TryFindClosestFloorTileTo(predicted, PredictionDistance + 2);
        }
    }
}
