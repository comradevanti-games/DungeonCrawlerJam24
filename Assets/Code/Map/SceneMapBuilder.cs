using System;
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

        private void BuildMap()
        {
            var blueprint = MapGen.Generate(new MapGen.Config(40, 20, 3, 5));
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
                tileGameObject.isStatic = true;
                var tileTransform = tileGameObject.transform;
                tileTransform.forward = TileSpaceMath.DirectionToWorldSpace(forward);
            }

            MapBuilt?.Invoke(new IMapBuilder.MapBuiltEvent(blueprint.FloorTiles));
        }

        private void Start()
        {
            BuildMap();
        }
    }
}
