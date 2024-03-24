using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using static DGJ24.TileSpace;

namespace DGJ24.Map
{
    internal static class MapGen
    {
        public record Config(int Width, int Height);

        private record MapInProgress(IImmutableSet<Vector2Int> FloorTiles);

        private static readonly MapInProgress emptyMap = new MapInProgress(
            ImmutableHashSet<Vector2Int>.Empty
        );

        private static MapBlueprint ToBlueprint(MapInProgress map)
        {
            return new MapBlueprint(map.FloorTiles);
        }

        private static MapInProgress PlaceFloorAt(MapInProgress map, Vector2Int tile)
        {
            return new MapInProgress(map.FloorTiles.Add(tile));
        }

        private static MapInProgress GeneratePaths(MapInProgress initialMap, Config config)
        {
            var bounds = MakeCenteredBounds(Vector2Int.zero, config.Width, config.Height);
            var paths = new HashSet<HashSet<Vector2Int>>();
            var potentialFloorTiles = new List<Vector2Int>();

            TilesInBounds(bounds)
                .ForEach(tile =>
                {
                    var xEven = tile.x % 2 == 0;
                    var yEven = tile.y % 2 == 0;
                    if (xEven && yEven)
                        paths.Add(new HashSet<Vector2Int> { tile });
                    else if (!(!xEven && !yEven))
                        potentialFloorTiles.Add(tile);
                });

            HashSet<Vector2Int>? TryGetPathAt(Vector2Int tile)
            {
                return paths.FirstOrDefault(path => path.Contains(tile));
            }

            while (potentialFloorTiles.Count > 0)
            {
                var tileIndex = Random.Range(0, potentialFloorTiles.Count);
                var tile = potentialFloorTiles[tileIndex];
                potentialFloorTiles.RemoveAt(tileIndex);

                var connectedPaths = CardinalNeighborsOf(tile)
                    .Select(TryGetPathAt)
                    .FilterNull()
                    .ToArray();

                if (connectedPaths.Length != 2)
                    continue;

                var pathA = connectedPaths[0]!;
                var pathB = connectedPaths[1]!;
                if (pathA == pathB)
                    continue;

                paths.Remove(pathA);
                pathB.UnionWith(pathA);
                pathB.Add(tile);
            }

            var allTiles = paths.SelectMany(it => it);
            return initialMap with { FloorTiles = initialMap.FloorTiles.Union(allTiles) };
        }

        public static MapBlueprint Generate(Config config)
        {
            var map = emptyMap;

            map = GeneratePaths(map, config);

            return ToBlueprint(map);
        }
    }
}
