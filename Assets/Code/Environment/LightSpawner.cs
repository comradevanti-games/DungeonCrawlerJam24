using System;
using System.Collections.Immutable;
using System.Linq;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.Environment
{
    internal class LightSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject lightPrefab = null!;

        [SerializeField]
        private int lightCount;

        [SerializeField]
        private float minLightDistance;

        private void SpawnLightOn(Vector2Int tile)
        {
            var position = TileSpaceMath.PositionToWorldSpace(tile);
            position.y = lightPrefab.transform.position.y;

            Instantiate(lightPrefab, position, lightPrefab.transform.rotation);
        }

        private void SpawnLights(IImmutableSet<Vector2Int> possibleTiles, int remaining)
        {
            if (remaining == 0 || possibleTiles.Count == 0)
                return;

            var tile =
                remaining == lightCount
                    ? Vector2Int.zero
                    : possibleTiles.ElementAt(Random.Range(0, possibleTiles.Count));

            SpawnLightOn(tile);

            SpawnLights(
                possibleTiles
                    .Where(it => Vector2Int.Distance(it, tile) >= minLightDistance)
                    .ToImmutableHashSet(),
                remaining - 1
            );
        }

        private void Awake()
        {
            Singletons.Get<IMapBuilder>().MapBuilt += @event =>
                SpawnLights(@event.FloorTiles, lightCount);
        }
    }
}
