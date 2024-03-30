using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class SceneTileSpaceEntityRepo
        : MonoBehaviour,
            ITileSpaceEntityRepo,
            ITileSpaceEntitySpawner
    {
        private record Entity(GameObject GameObject, ITileTransform Transform, bool HasCollision);

        [SerializeField]
        private GameObject[] initial = Array.Empty<GameObject>();

        private readonly ISet<Entity> entities = new HashSet<Entity>();

        public IEnumerable<GameObject> All => entities.Select(it => it.GameObject);

        public void Add(GameObject entity)
        {
            var tileTransform = entity.RequireComponent<ITileTransform>();
            var hasCollision = (bool)entity.GetComponent<TileCollider>();
            entities.Add(new Entity(entity, tileTransform, hasCollision));
        }

        private Entity? EntityAt(Vector2Int tile)
        {
            return entities.FirstOrDefault(it => it.Transform.Position == tile);
        }

        public GameObject? TryGetEntityOn(Vector2Int tile)
        {
            return EntityAt(tile)?.GameObject;
        }

        public bool EntityIsBlocking(Vector2Int tile)
        {
            var entity = EntityAt(tile);
            if (entity == null)
                return false;

            return entity.HasCollision;
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

        private void Update()
        {
            // Remove destroyed entities
            entities.ExceptWith(entities.Where(it => !it.GameObject).ToImmutableArray());
        }
    }
}
