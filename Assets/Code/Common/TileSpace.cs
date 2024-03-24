using System.Collections.Generic;
using UnityEngine;

namespace DGJ24
{
    /// <summary>
    /// Functions related to tile-space math.
    /// </summary>
    public static class TileSpace
    {
        public static readonly Vector2Int[] Deltas =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };

        public static IEnumerable<Vector2Int> CardinalNeighborsOf(Vector2Int tile)
        {
            yield return tile + Vector2Int.up;
            yield return tile + Vector2Int.right;
            yield return tile + Vector2Int.down;
            yield return tile + Vector2Int.left;
        }
    }
}
