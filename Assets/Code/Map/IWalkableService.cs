using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.Map
{
    /// <summary>
    /// Can answer queries about whether a tile is walkable or not.
    /// </summary>
    public interface IWalkableService
    {
        /// <summary>
        /// All currently walkable tiles.
        /// </summary>
        public IReadOnlyCollection<Vector2Int> WalkableTiles { get; }

        /// <summary>
        /// Checks if a specific position is walkable.
        /// </summary>
        /// <param name="tilePosition">The position to check.</param>
        /// <returns>Whether the tile is walkable</returns>
        public bool IsWalkable(Vector2Int tilePosition) => WalkableTiles.Contains(tilePosition);
    }
}
