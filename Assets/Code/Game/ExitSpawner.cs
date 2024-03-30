using System.Collections.Immutable;
using System.Linq;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Game
{
    internal class ExitSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject exitPrefab = null!;

        private ITileSpaceEntitySpawner entitySpawner = null!;

        private void SpawnExitAt(Vector2Int tile)
        {
            _ = entitySpawner.Spawn(exitPrefab, tile, CardinalDirection.Forward);
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
