using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.Map
{
    internal class SceneMapKeeper : MonoBehaviour, IWalkableService
    {
        private readonly ISet<Vector2Int> walkableTiles = new HashSet<Vector2Int>();

        public bool IsWalkable(Vector2Int tilePosition) => walkableTiles.Contains(tilePosition);

        private void OnMapBuilt(IMapBuilder.MapBuiltEvent args)
        {
            walkableTiles.Clear();
            foreach (var (position, tileType) in args.BuiltBlueprint.Tiles)
            {
                if (tileType == TileType.Floor)
                    walkableTiles.Add(position);
            }
        }

        private void Awake()
        {
            Singletons.Get<IMapBuilder>().MapBuilt += OnMapBuilt;
        }
    }
}
