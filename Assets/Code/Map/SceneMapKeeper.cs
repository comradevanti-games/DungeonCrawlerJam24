using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.Map
{
    internal class SceneMapKeeper : MonoBehaviour, IWalkableService
    {
        private readonly HashSet<Vector2Int> walkableTiles = new HashSet<Vector2Int>();

        public IReadOnlyCollection<Vector2Int> WalkableTiles => walkableTiles;

        private void OnMapBuilt(IMapBuilder.MapBuiltEvent args)
        {
            walkableTiles.Clear();
            walkableTiles.UnionWith(args.FloorTiles);
        }

        private void Awake()
        {
            Singletons.Get<IMapBuilder>().MapBuilt += OnMapBuilt;
        }
    }
}
