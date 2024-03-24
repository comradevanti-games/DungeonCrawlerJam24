using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

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

        private static MapInProgress GeneratePath(
            MapInProgress initialMap,
            Vector2Int startTile,
            Config config
        )
        {
            bool IsInBounds(Vector2Int tile)
            {
                return tile.x >= Mathf.Floor(-config.Width / 2f)
                    && tile.x <= Mathf.Ceil(config.Width / 2f)
                    && tile.y >= Mathf.Floor(-config.Height / 2f)
                    && tile.y <= Mathf.Ceil(config.Height / 2f);
            }

            bool CanContinuePathTo(MapInProgress map, Vector2Int tile)
            {
                return IsInBounds(tile) && !map.FloorTiles.Contains(tile);
            }

            MapInProgress TryContinue(MapInProgress map, Vector2Int currentTile)
            {
                map = PlaceFloorAt(map, currentTile);

                var possibleNextTiles = TileSpace
                    .CardinalNeighborsOf(currentTile)
                    .Where(tile => CanContinuePathTo(map, tile))
                    .ToImmutableArray();

                if (possibleNextTiles.IsEmpty)
                    return map;

                var nextTile = possibleNextTiles[Random.Range(0, possibleNextTiles.Length)];
                return TryContinue(map, nextTile);
            }

            return TryContinue(initialMap, startTile);
        }

        public static MapBlueprint Generate(Config config)
        {
            var map = emptyMap;

            map = GeneratePath(map, Vector2Int.zero, config);

            return ToBlueprint(map);
        }
    }
}
