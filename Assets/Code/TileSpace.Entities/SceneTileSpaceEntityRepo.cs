using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class SceneTileSpaceEntityRepo
        : MonoBehaviour,
            ITileSpaceEntityRepo,
            ITileSpaceEntitySpawner
    {
        private record Entity(GameObject GameObject, ITileTransform Transform);

        [SerializeField]
        private GameObject[] initial = Array.Empty<GameObject>();

        private readonly ISet<Entity> entities = new HashSet<Entity>();

        public IEnumerable<GameObject> All => entities.Select(it => it.GameObject);

        public void Add(GameObject entity)
        {
            var tileTransform = entity.RequireComponent<ITileTransform>();
            entities.Add(new Entity(entity, tileTransform));
        }

        public bool HasEntityAt(Vector2Int tile)
        {
            return entities.Any(it => it.Transform.Position == tile);
        }

        public GameObject Spawn(GameObject prefab, Vector2Int tile, CardinalDirection forward)
        {
            var position = TileSpaceMath.PositionToWorldSpace(tile);
            var rotation = Quaternion.LookRotation(TileSpaceMath.DirectionToWorldSpace(forward));

            var entity = Instantiate(prefab, position, rotation);
            var tileTransform = entity.RequireComponent<ITileTransform>();
            tileTransform.Position = tile;
            tileTransform.Forward = forward;

            Add(entity);

            return entity;
        }

        private void Awake()
        {
            initial.ForEach(Add);
        }
    }
}
