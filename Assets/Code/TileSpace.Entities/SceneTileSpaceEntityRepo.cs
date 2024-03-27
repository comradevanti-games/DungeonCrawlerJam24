using System;
using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class SceneTileSpaceEntityRepo : MonoBehaviour, ITileSpaceEntityRepo
    {
        [SerializeField]
        private GameObject[] initial = Array.Empty<GameObject>();

        private readonly ISet<GameObject> entities = new HashSet<GameObject>();

        public IEnumerable<GameObject> All => entities;

        public void Add(GameObject entity)
        {
            entities.Add(entity);
        }

        private void Awake()
        {
            initial.ForEach(Add);
        }
    }
}
