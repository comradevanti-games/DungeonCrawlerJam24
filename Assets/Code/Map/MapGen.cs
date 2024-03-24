using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    internal static class MapGen
    {
        public record Config;

        private static readonly string[] layoutStrings =
        {
            "   fffff ",
            " ffff  ff",
            " f ffffff",
            " f  f  f ",
            " fffffff ",
            "   fffff "
        };

        public static MapBlueprint Generate(Config config)
        {
            var floorTiles = new HashSet<Vector2Int>();

            for (var x = 0; x < layoutStrings.Length; x++)
            {
                var line = layoutStrings[x];
                for (var y = 0; y < line.Length; y++)
                {
                    var c = line[y];
                    if (c == ' ')
                        continue;

                    floorTiles.Add(new Vector2Int(x, y));
                }
            }

            return new MapBlueprint(floorTiles.ToImmutableHashSet());
        }
    }
}
