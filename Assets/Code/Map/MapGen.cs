using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using static DGJ24.TileSpace;
using Random = UnityEngine.Random;

namespace DGJ24.Map
{
    internal static class MapGen
    {
        public record Config(
            int Width,
            int Height,
            int MinRoomSize,
            int MaxRoomSize,
            int EnemyCount
        );

        private record MapInProgress(
            IImmutableSet<Vector2Int> FloorTiles,
            IImmutableSet<Vector2Int> EnemyPositions
        );

        private static readonly MapInProgress emptyMap = new MapInProgress(
            ImmutableHashSet<Vector2Int>.Empty,
            ImmutableHashSet<Vector2Int>.Empty
        );

        private static MapBlueprint ToBlueprint(MapInProgress map)
        {
            return new MapBlueprint(map.FloorTiles, map.EnemyPositions);
        }

        private static int RoomCountFor(RectInt bounds, int minRoomSize, int maxRoomSize)
        {
            var averageSize = (maxRoomSize + minRoomSize) / 2f;
            var averageArea = averageSize * averageSize;
            var mapArea = bounds.width * bounds.height;
            var areaUsedByRooms = mapArea * 0.4f;
            return Mathf.FloorToInt(areaUsedByRooms / averageArea);
        }

        private static bool MapHasFloorIn(RectInt bounds, MapInProgress map)
        {
            return map.FloorTiles.Any(bounds.Contains);
        }

        private static MapInProgress PlaceRoomAt(RectInt bounds, MapInProgress map)
        {
            var tiles = TilesInBounds(bounds);
            return map with { FloorTiles = map.FloorTiles.Union(tiles) };
        }

        private static MapInProgress GenerateRooms(
            MapInProgress initial,
            RectInt bounds,
            int minRoomSize,
            int maxRoomSize
        )
        {
            var roomCount = RoomCountFor(bounds, minRoomSize, maxRoomSize);

            return Enumerable
                .Range(0, roomCount)
                .Aggregate(
                    initial,
                    (map, _) =>
                    {
                        var size = new Vector2Int(
                            Random.Range(minRoomSize, maxRoomSize),
                            Random.Range(minRoomSize, maxRoomSize)
                        );

                        RectInt roomBounds;
                        var triesLeft = 100;
                        do
                        {
                            var targetPosition = new Vector2Int(
                                Random.Range(bounds.xMin, bounds.xMax - size.x),
                                Random.Range(bounds.yMin, bounds.yMax - size.y)
                            );
                            roomBounds = new RectInt(targetPosition, size);
                            triesLeft--;
                        } while (MapHasFloorIn(roomBounds, map) && triesLeft > 0);

                        return triesLeft == 0 ? map : PlaceRoomAt(roomBounds, map);
                    }
                );
        }

        private static MapInProgress GeneratePaths(MapInProgress initialMap, RectInt bounds)
        {
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

        private static MapInProgress RemoveDeadEnds(MapInProgress initialMap)
        {
            var newFloorTiles = new HashSet<Vector2Int>();

            initialMap.FloorTiles.ForEach(tile =>
            {
                var neighbors = CardinalNeighborsOf(tile).ToHashSet();
                var occupiedNeighbors = neighbors.Where(initialMap.FloorTiles.Contains);

                var isDeadEnd = occupiedNeighbors.Count() == 1;
                if (!isDeadEnd)
                    return;

                var possibleConnections = neighbors
                    .Where(it => !initialMap.FloorTiles.Contains(it))
                    .Where(it =>
                        CardinalNeighborsOf(it).Except(tile).Any(initialMap.FloorTiles.Contains)
                    )
                    .ToImmutableArray();

                var newTile = possibleConnections[Random.Range(0, possibleConnections.Length)];
                newFloorTiles.Add(newTile);
            });

            return initialMap with
            {
                FloorTiles = initialMap.FloorTiles.Union(newFloorTiles)
            };
        }

        private static MapInProgress PlaceEnemy(MapInProgress map)
        {
            var potentialEnemyTiles = map
                .FloorTiles.Except(map.EnemyPositions)
                .Where(tile => tile.magnitude > 10f)
                .ToImmutableArray();

            if (potentialEnemyTiles.Length == 0)
                throw new Exception("Map too small to spawn enemy");

            var index = Random.Range(0, potentialEnemyTiles.Length);
            var tile = potentialEnemyTiles[index];

            return map with
            {
                EnemyPositions = map.EnemyPositions.Add(tile)
            };
        }

        public static MapBlueprint Generate(Config config)
        {
            var bounds = MakeCenteredBounds(Vector2Int.zero, config.Width, config.Height);
            var map = emptyMap;

            map = GenerateRooms(map, bounds, config.MinRoomSize, config.MaxRoomSize);
            map = GeneratePaths(map, bounds);
            map = RemoveDeadEnds(map);

            for (var i = 0; i < config.EnemyCount; i++)
                map = PlaceEnemy(map);

            return ToBlueprint(map);
        }
    }
}
