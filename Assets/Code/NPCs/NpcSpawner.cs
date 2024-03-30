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
        private GameObject[] prefabs = Array.Empty<GameObject>();

        private ITileSpaceEntitySpawner entitySpawner = null!;

        private void SpawnNpcAt(Vector2Int tile, GameObject prefab)
        {
            var npc = entitySpawner.Spawn(prefab, tile, CardinalDirection.Forward);
            NpcSpawned?.Invoke(new INpcSpawner.NpcSpawnedEvent(npc));
        }

        private void SpawnNpc(
            IImmutableSet<Vector2Int> possibleTiles,
            IImmutableList<GameObject> remaining
        )
        {
            if (remaining.Count == 0)
                return;

            var prefab = remaining[0];
            var tile = possibleTiles.ElementAt(Random.Range(0, possibleTiles.Count));

            SpawnNpcAt(tile, prefab);

            SpawnNpc(possibleTiles.Remove(tile), remaining.RemoveAt(0));
        }

        private void Awake()
        {
            entitySpawner = Singletons.Get<ITileSpaceEntitySpawner>();
            Singletons.Get<IMapBuilder>().MapBuilt += @event =>
                SpawnNpc(
                    @event.FloorTiles.Where(it => it.magnitude >= 10).ToImmutableHashSet(),
                    prefabs.ToImmutableList()
                );
        }
    }
}
