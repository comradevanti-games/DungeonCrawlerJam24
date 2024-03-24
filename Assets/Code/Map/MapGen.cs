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
            var blueprint = MapBlueprint.empty;

            for (var x = 0; x < layoutStrings.Length; x++)
            {
                var line = layoutStrings[x];
                for (var y = 0; y < line.Length; y++)
                {
                    var c = line[y];
                    if (c == ' ')
                        continue;

                    blueprint = MapBlueprint.PlaceFloorAt(blueprint, new Vector2Int(x, y));
                }
            }

            return blueprint;
        }
    }
}
