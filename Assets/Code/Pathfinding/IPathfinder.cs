using UnityEngine;

namespace DGJ24.Pathfinding
{
    /// <summary>
    /// Functions for path-finding in the map.
    /// </summary>
    public interface IPathfinder
    {
        /// <summary>
        /// Attempts to find a path between two points on the map.
        /// </summary>
        /// <param name="from">The start tile</param>
        /// <param name="to">The end tile</param>
        /// <returns>The path or null if the target cannot be reached.</returns>
        public Path? TryFindPath(Vector2Int from, Vector2Int to);
    }
}
