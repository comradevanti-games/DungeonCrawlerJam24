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
            var blueprint = MapGen.Generate(new MapGen.Config());

            foreach (var tilePosition in blueprint.FloorTiles)
            {
                var position = new Vector3(
                    tilePosition.x,
                    floorPrefab.transform.position.y,
                    tilePosition.y
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
