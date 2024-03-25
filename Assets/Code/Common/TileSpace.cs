using System;
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
        public static readonly Vector2Int[] CardinalDirections =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

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
            return CardinalDirections.Select(dir => tile + dir);
        }

        public static RectInt MakeCenteredBounds(Vector2Int center, int width, int height)
        {
            var minX = center.x - Mathf.FloorToInt((width - 1) / 2f);
            var minY = center.y - Mathf.FloorToInt((height - 1) / 2f);

            return new RectInt(minX, minY, width, height);
        }

        public static IEnumerable<Vector2Int> TilesInBounds(RectInt bounds)
        {
            for (var x = bounds.xMin; x < bounds.xMax; x++)
            for (var y = bounds.yMin; y < bounds.yMax; y++)
                yield return new Vector2Int(x, y);
        }

        public static RectInt Grow(RectInt bounds, int border)
        {
            return new RectInt(
                bounds.xMin - border,
                bounds.yMin - border,
                bounds.width + border * 2,
                bounds.height + border * 2
            );
        }

        public static Vector2Int GetVectorForDirection(GridDirection direction) =>
            direction switch
            {
                GridDirection.ZPlus => Vector2Int.up,
                GridDirection.XPlus => Vector2Int.right,
                GridDirection.ZMinus => Vector2Int.down,
                GridDirection.XMinus => Vector2Int.left,

                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

        public static Vector2Int GetDestinationTile(Vector2Int origin, GridDirection direction)
        {
            return origin + GetVectorForDirection(direction);
        }

        public static Vector3 DirectionToWorldSpace(Vector2Int dir) => new Vector3(dir.x, 0, dir.y);

        public static Vector3 PositionToWorldSpace(Vector2Int dir) =>
            new Vector3(dir.x * 2, 0, dir.y * 2);
    }
}
