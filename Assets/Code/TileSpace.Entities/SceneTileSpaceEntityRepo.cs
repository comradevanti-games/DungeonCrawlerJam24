using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class SceneTileSpaceEntityRepo : MonoBehaviour, ITileSpaceEntityRepo
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

        private void Awake()
        {
            initial.ForEach(Add);
        }
    }
}
