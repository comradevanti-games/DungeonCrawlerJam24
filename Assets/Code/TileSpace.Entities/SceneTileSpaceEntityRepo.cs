using System;
using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class SceneTileSpaceEntityRepo : MonoBehaviour, ITileSpaceEntityRepo
    {
        [SerializeField]
        private GameObject[] initial = Array.Empty<GameObject>();

        private readonly ISet<ITileSpaceEntity> entities = new HashSet<ITileSpaceEntity>();

        public IEnumerable<ITileSpaceEntity> All => entities;

        public bool TryAdd(GameObject entityGameObject)
        {
            var entity = TileSpaceGameObject.TryMakeFrom(entityGameObject);
            if (entity == null)
                return false;

            entities.Add(entity);
            return true;
        }

        private void Awake()
        {
            initial.ForEach(((ITileSpaceEntityRepo)this).AddOrThrow);
        }
    }
}
