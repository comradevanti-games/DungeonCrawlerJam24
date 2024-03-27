using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.TileSpace
{
    /// <summary>
    /// Functions related to tile-space math.
    /// </summary>
    public static class TileSpaceMath
    {
        public static readonly Vector2Int[] CardinalDirectionVectors =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        public static readonly Vector2Int[] DirectionVectors =
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
            return CardinalDirectionVectors.Select(dir => tile + dir);
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

        public static Vector2Int VectorForDirection(CardinalDirection direction) =>
            CardinalDirectionVectors[(int)direction];

        public static CardinalDirection? TryDirectionForVector(Vector2Int vector)
        {
            return (vector.x, vector.y)switch
            {
                (0, 1) => CardinalDirection.Forward,
                (1, 0) => CardinalDirection.Right,
                (0, -1) => CardinalDirection.Backward,
                (-1, 0) => CardinalDirection.Left,
                _ => null
            };
        }
        
        public static Vector2Int MoveByDirection(Vector2Int tile, CardinalDirection direction)
        {
            return tile + VectorForDirection(direction);
        }

        public static Vector3 DirectionToWorldSpace(CardinalDirection direction)
        {
            var dir = VectorForDirection(direction);
            return new Vector3(dir.x, 0, dir.y);
        }

        public static Vector3 PositionToWorldSpace(Vector2Int tile) =>
            new Vector3(tile.x * 2, 0, tile.y * 2);

        public static CardinalDirection RotateDirection(
            CardinalDirection direction,
            RotationDirection rotation
        )
        {
            var unrepeated = (int)direction + (int)rotation;
            var repeated = unrepeated switch
            {
                -1 => 3,
                4 => 0,
                _ => unrepeated
            };
            return (CardinalDirection)repeated;
        }
    }
}
