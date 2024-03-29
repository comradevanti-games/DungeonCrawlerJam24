using UnityEngine;

namespace DGJ24.AI
{
    public interface ITargetTileProvider
    {
        public Vector2Int? Tile { get; }
    }
}