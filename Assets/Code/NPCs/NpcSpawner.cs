using System;
using System.Collections.Immutable;
using System.Linq;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.NPCs
{
    internal class NpcSpawner : MonoBehaviour, INpcSpawner
    {
        public event Action<INpcSpawner.NpcSpawnedEvent>? NpcSpawned;

        [SerializeField]
        private int enemyCount;

        [SerializeField]
        private GameObject prefab = null!;

        private ITileSpaceEntitySpawner entitySpawner = null!;

        private void SpawnEnemy(Vector2Int tile)
        {
            var npc = entitySpawner.Spawn(prefab, tile, CardinalDirection.Forward);
            NpcSpawned?.Invoke(new INpcSpawner.NpcSpawnedEvent(npc));
        }

        private void SpawnLoot(IImmutableSet<Vector2Int> possibleTiles, int remainingCount)
        {
            if (remainingCount == 0)
                return;
            var tile = possibleTiles.ElementAt(Random.Range(0, possibleTiles.Count));

            SpawnEnemy(tile);

            SpawnLoot(possibleTiles.Remove(tile), remainingCount - 1);
        }

        private void Awake()
        {
            entitySpawner = Singletons.Get<ITileSpaceEntitySpawner>();
            Singletons.Get<IMapBuilder>().MapBuilt += @event =>
                SpawnLoot(@event.FloorTiles, enemyCount);
        }
    }
}
