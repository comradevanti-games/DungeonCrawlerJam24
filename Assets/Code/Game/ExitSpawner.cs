using System.Collections.Immutable;
using System.Linq;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Game
{
    internal class ExitSpawner : MonoBehaviour
    {
        public record ExitSpawnedArgs(GameObject Exit);

        [SerializeField]
        private UnityEvent<ExitSpawnedArgs> exitSpawned = new UnityEvent<ExitSpawnedArgs>();

        [SerializeField]
        private GameObject exitPrefab = null!;

        private ITileSpaceEntitySpawner entitySpawner = null!;

        private void SpawnExitAt(Vector2Int tile)
        {
            var exit = entitySpawner.Spawn(exitPrefab, tile, CardinalDirection.Forward);
            exitSpawned.Invoke(new ExitSpawnedArgs(exit));
        }

        private void SpawnExit(IImmutableSet<Vector2Int> possibleTiles)
        {
            var tile = possibleTiles.ElementAt(Random.Range(0, possibleTiles.Count));

            SpawnExitAt(tile);
        }

        private void Awake()
        {
            entitySpawner = Singletons.Get<ITileSpaceEntitySpawner>();
            Singletons.Get<IMapBuilder>().MapBuilt += @event =>
                SpawnExit(@event.FloorTiles.Where(it => it.magnitude >= 10).ToImmutableHashSet());
        }
    }
}
