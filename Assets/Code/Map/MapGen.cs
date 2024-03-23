using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace DGJ24.Map
{
    internal static class MapGen
    {
        public record Config;

        private static readonly string[] layoutStrings =
        {
            "wwwwww   ",
            "wffffwwww",
            "wfwfffffw",
            "wfwwfwwfw",
            "wfffffffw",
            "wwwwwwwww"
        };

        public static MapBlueprint Generate(Config config)
        {
            var tiles = new Dictionary<Vector2Int, TileType>();

            for (var x = 0; x < layoutStrings.Length; x++)
            {
                var line = layoutStrings[x];
                for (var y = 0; y < line.Length; y++)
                {
                    var c = line[y];
                    if (c == ' ')
                        continue;

                    var tileType = c switch
                    {
                        'w' => TileType.Wall,
                        'f' => TileType.Floor,
                        _ => throw new Exception("Unknown tile-type")
                    };

                    tiles.Add(new Vector2Int(x, y), tileType);
                }
            }

            return new MapBlueprint(tiles.ToImmutableDictionary());
        }
    }
}
