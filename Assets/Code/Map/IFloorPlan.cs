using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.Map
{
    /// <summary>
    /// Holds information about the floor-tiles of the current map.
    /// </summary>
    public interface IFloorPlan
    {
        /// <summary>
        /// All currently walkable tiles.
        /// </summary>
        public IReadOnlyCollection<Vector2Int> Tiles { get; }

        /// <summary>
        /// Checks if a specific tile is part of the floor-plan.
        /// </summary>
        public bool Contains(Vector2Int tile) => Tiles.Contains(tile);
    }
}
