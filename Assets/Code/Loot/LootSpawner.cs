using System.Collections.Immutable;
using System.Linq;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Loot
{
    internal class LootSpawner : MonoBehaviour
    {
        [SerializeField]
        private int lootCount;

        [SerializeField]
        private GameObject lootPrefab = null!;

        private ITileSpaceEntitySpawner entitySpawner = null!;

        private void SpawnLootAt(Vector2Int tile)
        {
            _ = entitySpawner.Spawn(lootPrefab, tile, CardinalDirection.Forward);
        }

        private void SpawnLoot(IImmutableSet<Vector2Int> possibleTiles, int remainingCount)
        {
            if (remainingCount == 0)
                return;
            var tile = possibleTiles.ElementAt(Random.Range(0, possibleTiles.Count));

            SpawnLootAt(tile);

            SpawnLoot(possibleTiles.Remove(tile), remainingCount - 1);
        }

        private void Awake()
        {
            entitySpawner = Singletons.Get<ITileSpaceEntitySpawner>();
            Singletons.Get<IMapBuilder>().MapBuilt += @event =>
                SpawnLoot(@event.FloorTiles, lootCount);
        }
    }
}
