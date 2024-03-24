using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Vector2Int> DiagonalNeighborsOf(Vector2Int tile)
        {
            yield return tile + new Vector2Int(1, 1);
            yield return tile + new Vector2Int(1, -1);
            yield return tile + new Vector2Int(-1, -1);
            yield return tile + new Vector2Int(-1, 1);
        }

        public static IEnumerable<Vector2Int> AllNeighborsOf(Vector2Int tile) =>
            CardinalNeighborsOf(tile).Concat(DiagonalNeighborsOf(tile));

        public static RectInt MakeCenteredBounds(Vector2Int center, int width, int height)
        {
            var minX = center.x - Mathf.FloorToInt((width - 1) / 2f);
            var minY = center.y - Mathf.FloorToInt((height - 1) / 2f);

            return new RectInt(minX, minY, width, height);
        }

        public static IEnumerable<Vector2Int> TilesInBounds(RectInt bounds)
        {
            for (var x = bounds.xMin; x <= bounds.xMax; x++)
            for (var y = bounds.yMin; y <= bounds.yMax; y++)
                yield return new Vector2Int(x, y);
        }
    }
}
