using System;
using UnityEngine;

namespace DGJ24.Map
{
    internal class SceneMapBuilder : MonoBehaviour, IMapBuilder
    {
        public event Action<IMapBuilder.MapBuiltEvent>? MapBuilt;

        [SerializeField]
        private GameObject floorPrefab = null!;

        private void BuildMap()
        {
            var blueprint = MapGen.Generate(new MapGen.Config(40, 20, 3, 8));

            foreach (var tilePosition in blueprint.FloorTiles)
            {
                var position = new Vector3(
                    tilePosition.x * 2,
                    floorPrefab.transform.position.y,
                    tilePosition.y * 2
                );

                Instantiate(floorPrefab, position, Quaternion.identity);
            }

            MapBuilt?.Invoke(new IMapBuilder.MapBuiltEvent(blueprint));
        }

        private void Start()
        {
            BuildMap();
        }
    }
}
