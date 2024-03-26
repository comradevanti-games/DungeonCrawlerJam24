using System;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.Map
{
    internal class SceneMapBuilder : MonoBehaviour, IMapBuilder
    {
        public event Action<IMapBuilder.MapBuiltEvent>? MapBuilt;

        [SerializeField]
        private OTiling.TileSet tileSet = new OTiling.TileSet();

        [SerializeField]
        private GameObject fallbackPrefab = null!;

        [SerializeField]
        private GameObject enemyPrefab = null!;

        private void BuildMap()
        {
            var blueprint = MapGen.Generate(new MapGen.Config(40, 20, 3, 5, 3, 10));
            var prefabsByMask = tileSet.Compute();

            foreach (var tile in blueprint.FloorTiles)
            {
                var position = TileSpace.PositionToWorldSpace(tile);

                var tileMask = OTiling.TilingMaskFor(blueprint, tile);
                var options = prefabsByMask[tileMask];
                var (prefab, forward) =
                    options.Length > 0
                        ? options[Random.Range(0, options.Length)]
                        : (fallbackPrefab, GridDirection.ZPlus);

                var tileGameObject = Instantiate(prefab, position, Quaternion.identity);
                var tileTransform = tileGameObject.transform;
                tileTransform.forward = TileSpace.DirectionToWorldSpace(
                    TileSpace.GetVectorForDirection(forward)
                );
            }

            var enemies = blueprint
                .EnemyTiles.Select(TileSpace.PositionToWorldSpace)
                .Select(position => Instantiate(enemyPrefab, position, Quaternion.identity))
                .ToImmutableHashSet();

            MapBuilt?.Invoke(new IMapBuilder.MapBuiltEvent(blueprint.FloorTiles, enemies));
        }

        private void Start()
        {
            BuildMap();
        }
    }
}
