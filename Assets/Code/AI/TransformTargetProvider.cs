using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal record TransformTargetProvider(ITileTransform Transform) : ITargetTileProvider
    {
        public Vector2Int? Tile => Transform.Position;
    }
}
