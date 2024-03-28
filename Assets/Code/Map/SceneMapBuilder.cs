using System;
using System.Collections.Immutable;
using System.Linq;
using DGJ24.TileSpace;
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


        private ITileSpaceEntityRepo tileSpaceEntityRepo;

        private void BuildMap()
        {
            var blueprint = MapGen.Generate(new MapGen.Config(40, 20, 3, 5, 3));
            var prefabsByMask = tileSet.Compute();

            foreach (var tile in blueprint.FloorTiles)
            {
                var position = TileSpaceMath.PositionToWorldSpace(tile);

                var tileMask = OTiling.TilingMaskFor(blueprint, tile);
                var options = prefabsByMask[tileMask];
                var (prefab, forward) =
                    options.Length > 0
                        ? options[Random.Range(0, options.Length)]
                        : (fallbackPrefab, CardinalDirection.Forward);

                var tileGameObject = Instantiate(prefab, position, Quaternion.identity);
                var tileTransform = tileGameObject.transform;
                tileTransform.forward = TileSpaceMath.DirectionToWorldSpace(forward);
            }

            var enemies = blueprint
                .EnemyTiles.Select(tile =>
                {
                    var position = TileSpaceMath.PositionToWorldSpace(tile);
                    var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                    enemy.RequireComponent<ITileTransform>().Position = tile;
                    return enemy;
                })
                .ToImmutableHashSet();
            enemies.ForEach(tileSpaceEntityRepo.Add);

            MapBuilt?.Invoke(new IMapBuilder.MapBuiltEvent(blueprint.FloorTiles, enemies));
        }

        private void Start()
        {
            BuildMap();
        }

        private void Awake()
        {
            tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
        }
    }
}
