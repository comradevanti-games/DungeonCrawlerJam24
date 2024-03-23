using System;
using UnityEngine;

namespace DGJ24.Map
{
    internal class SceneMapBuilder : MonoBehaviour, IMapBuilder
    {
        public event Action<IMapBuilder.MapBuiltEvent>? MapBuilt;

        [SerializeField]
        private GameObject floorPrefab = null!;

        [SerializeField]
        private GameObject wallPrefab = null!;

        private void BuildMap()
        {
            var blueprint = MapGen.Generate(new MapGen.Config());

            foreach (var (tilePosition, tileType) in blueprint.Tiles)
            {
                var prefab = tileType switch
                {
                    TileType.Wall => wallPrefab,
                    TileType.Floor => floorPrefab,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var position = new Vector3(
                    tilePosition.x,
                    prefab.transform.position.y,
                    tilePosition.y
                );

                Instantiate(prefab, position, Quaternion.identity);
            }

            MapBuilt?.Invoke(new IMapBuilder.MapBuiltEvent(blueprint));
        }

        private void Start()
        {
            BuildMap();
        }
    }
}
