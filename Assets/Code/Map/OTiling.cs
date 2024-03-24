using System;
using UnityEngine;

namespace DGJ24.Map
{
    /// <summary>
    /// Utility functions related to o-tiling.
    /// </summary>
    internal static class OTiling
    {
        private static readonly Vector2Int[] deltas =
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

        public static byte MaskKeyFor(Vector2Int delta)
        {
            return (delta.x, delta.y) switch
            {
                (0, 1) => 1,
                (1, 1) => 2,
                (1, 0) => 4,
                (1, -1) => 8,
                (0, -1) => 16,
                (-1, -1) => 32,
                (-1, 0) => 64,
                (-1, 1) => 128,
                _ => throw new ArgumentException("Bad delta for mask-key!")
            };
        }

        public static byte TilingMaskFor(MapBlueprint map, Vector2Int position)
        {
            byte mask = 0;

            foreach (var delta in deltas)
            {
                var borderPos = position + delta;
                var isWall = !map.FloorTiles.Contains(borderPos);

                if (isWall)
                    mask += MaskKeyFor(delta);
            }

            return mask;
        }
    }
}
