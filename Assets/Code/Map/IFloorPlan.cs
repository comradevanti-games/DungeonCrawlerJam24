using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DGJ24.TileSpace;
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

        public bool IsBlocking(Vector2Int tile)
        {
            var neighbors = TileSpaceMath.AllNeighborsOf(tile).Where(Contains).ToImmutableHashSet();
            return !neighbors.All(neighbor =>
                TileSpaceMath.CardinalNeighborsOf(neighbor).Any(neighbors.Contains)
            );
        }

        public Vector2Int? TryFindClosestFloorTileTo(Vector2Int tile, int maxSearchDistance)
        {
            Vector2Int? TryFindIn(int distance)
            {
                if (distance > maxSearchDistance)
                    return null;

                for (var dx = -distance; dx <= distance; dx++)
                for (var dy = -distance; dy <= distance; dy++)
                {
                    var searchTile = tile + new Vector2Int(dx, dy);
                    if (Contains(searchTile))
                        return searchTile;
                }

                return TryFindIn(distance + 1);
            }

            return TryFindIn(0);
        }
    }
}
