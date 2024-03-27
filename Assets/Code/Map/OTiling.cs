using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Map
{
    /// <summary>
    /// Utility functions related to o-tiling.
    /// </summary>
    internal static class OTiling
    {
        [Serializable]
        internal class TileSet
        {
            [Serializable]
            internal class Item
            {
                public GameObject prefab = null!;
                public byte wallMask;
                public byte floorMask;
            }

            [SerializeField]
            private Item[] items = Array.Empty<Item>();

            private static byte RotateLeft(byte b, int times)
            {
                return (byte)(b >> times | b << (8 - times));
            }

            public IImmutableDictionary<byte, ImmutableArray<(GameObject, GridDirection)>> Compute()
            {
                var dict = new Dictionary<byte, IList<(GameObject, GridDirection)>>();

                for (var i = 0; i <= 255; i++)
                {
                    var mask = (byte)i;
                    var options = new List<(GameObject, GridDirection)>();

                    for (var o = 0; o < 4; o++)
                    {
                        var rotatedMask = RotateLeft(mask, o * 2);
                        var dir = (GridDirection)o;
                        foreach (var item in items)
                        {
                            var wallMask = (byte)~rotatedMask;
                            var floorMask = rotatedMask;

                            var wallMaskMatches = (wallMask & item.wallMask) == item.wallMask;
                            var floorMaskMatches = (floorMask & item.floorMask) == item.floorMask;

                            if (wallMaskMatches && floorMaskMatches)
                                options.Add((item.prefab, dir));
                        }
                    }

                    dict.Add(mask, options);
                }

                return dict.ToImmutableDictionary(kv => kv.Key, kv => kv.Value.ToImmutableArray());
            }
        }

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

            foreach (var delta in TileSpaceMath.Deltas)
            {
                var borderPos = position + delta;
                var isFloor = map.FloorTiles.Contains(borderPos);

                if (isFloor)
                    mask += MaskKeyFor(delta);
            }

            return mask;
        }
    }
}
