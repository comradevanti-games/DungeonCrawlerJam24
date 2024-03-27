using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using static DGJ24.TileSpace.TileSpaceMath;
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
            int EnemyCount,
            int LootCount
        );

        private record FloorPlan(IImmutableSet<Vector2Int> Tiles)
        {
            public static readonly FloorPlan Empty = new FloorPlan(
                ImmutableHashSet<Vector2Int>.Empty
            );

            public bool HasFloorAt(Vector2Int tile) => Tiles.Contains(tile);

            public bool HasFloorIn(RectInt bounds) => Tiles.Any(bounds.Contains);

            public FloorPlan SetFloorAtAll(IEnumerable<Vector2Int> tiles) =>
                this with
                {
                    Tiles = Tiles.Union(tiles)
                };

            public FloorPlan SetFloorIn(RectInt bounds)
            {
                var newTiles = TilesInBounds(bounds);
                return SetFloorAtAll(newTiles);
            }
        }

        private record EnemyPlan(IImmutableSet<Vector2Int> Tiles)
        {
            public static readonly EnemyPlan Empty = new EnemyPlan(
                ImmutableHashSet<Vector2Int>.Empty
            );

            public EnemyPlan AddEnemyAt(Vector2Int tile) => this with { Tiles = Tiles.Add(tile) };
        }

        private record LootPlan(IImmutableSet<Vector2Int> Tiles)
        {
            public static readonly LootPlan Empty = new LootPlan(
                ImmutableHashSet<Vector2Int>.Empty
            );

            public LootPlan AddLootAt(Vector2Int tile)
            {
                return this with { Tiles = Tiles.Add(tile) };
            }
        }

        private static int RoomCountFor(RectInt bounds, int minRoomSize, int maxRoomSize)
        {
            var averageSize = (maxRoomSize + minRoomSize) / 2f;
            var averageArea = averageSize * averageSize;
            var mapArea = bounds.width * bounds.height;
            var areaUsedByRooms = mapArea * 0.4f;
            return Mathf.FloorToInt(areaUsedByRooms / averageArea);
        }

        private static FloorPlan TryGenerateRoom(
            FloorPlan floorPlan,
            RectInt bounds,
            int minRoomSize,
            int maxRoomSize
        )
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
            } while (floorPlan.HasFloorIn(roomBounds) && triesLeft > 0);

            return triesLeft == 0 ? floorPlan : floorPlan.SetFloorIn(roomBounds);
        }

        private static FloorPlan GeneratePaths(FloorPlan floorPlan, RectInt bounds)
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
            return floorPlan.SetFloorAtAll(allTiles);
        }

        private static FloorPlan RemoveDeadEnds(FloorPlan floorPlan)
        {
            var newFloorTiles = new HashSet<Vector2Int>();

            floorPlan.Tiles.ForEach(tile =>
            {
                var neighbors = CardinalNeighborsOf(tile).ToHashSet();
                var occupiedNeighbors = neighbors.Where(floorPlan.HasFloorAt);

                var isDeadEnd = occupiedNeighbors.Count() == 1;
                if (!isDeadEnd)
                    return;

                var possibleConnections = neighbors
                    .Where(it => !floorPlan.Tiles.Contains(it))
                    .Where(it => CardinalNeighborsOf(it).Except(tile).Any(floorPlan.HasFloorAt))
                    .ToImmutableArray();

                var newTile = possibleConnections[Random.Range(0, possibleConnections.Length)];
                newFloorTiles.Add(newTile);
            });

            return floorPlan.SetFloorAtAll(newFloorTiles);
        }

        private static EnemyPlan PlaceEnemy(FloorPlan floorPlan, EnemyPlan enemyPlan)
        {
            var potentialEnemyTiles = floorPlan
                .Tiles.Except(enemyPlan.Tiles)
                .Where(tile => tile.magnitude > 10f)
                .ToImmutableArray();

            if (potentialEnemyTiles.Length == 0)
                throw new Exception("Map too small to spawn enemy");

            var index = Random.Range(0, potentialEnemyTiles.Length);
            var tile = potentialEnemyTiles[index];

            return enemyPlan.AddEnemyAt(tile);
        }

        private static LootPlan PlaceLoot(FloorPlan floorPlan, LootPlan lootPlan)
        {
            float DistanceToNextLoot(Vector2Int tile)
            {
                if (lootPlan.Tiles.Count == 0)
                    return float.PositiveInfinity;
                return lootPlan.Tiles.Select(lootTile => Vector2Int.Distance(lootTile, tile)).Min();
            }

            var potentialTiles = floorPlan
                .Tiles.Where(tile => DistanceToNextLoot(tile) >= 5f)
                .ToImmutableArray();

            if (potentialTiles.Length == 0)
                throw new Exception("Map to small to spawn loot");

            var index = Random.Range(0, potentialTiles.Length);
            var tile = potentialTiles[index];

            return lootPlan.AddLootAt(tile);
        }

        public static MapBlueprint Generate(Config config)
        {
            var bounds = MakeCenteredBounds(Vector2Int.zero, config.Width, config.Height);
            var floorPlan = FloorPlan.Empty;

            var roomCount = RoomCountFor(bounds, config.MinRoomSize, config.MaxRoomSize);
            for (var i = 0; i < roomCount; i++)
                floorPlan = TryGenerateRoom(
                    floorPlan,
                    bounds,
                    config.MinRoomSize,
                    config.MinRoomSize
                );

            floorPlan = GeneratePaths(floorPlan, bounds);
            floorPlan = RemoveDeadEnds(floorPlan);

            var lootPlan = LootPlan.Empty;
            for (var i = 0; i < config.LootCount; i++)
                lootPlan = PlaceLoot(floorPlan, lootPlan);

            var enemyPlan = EnemyPlan.Empty;
            for (var i = 0; i < config.EnemyCount; i++)
                enemyPlan = PlaceEnemy(floorPlan, enemyPlan);

            return new MapBlueprint(floorPlan.Tiles, enemyPlan.Tiles, lootPlan.Tiles);
        }
    }
}
