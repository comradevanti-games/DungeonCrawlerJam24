using UnityEngine;

namespace DGJ24.Navigation
{
    /// <summary>
    /// Provides information about whether a specific tile is walkable.
    /// </summary>
    public interface IWalkableProvider
    {
        public bool IsWalkable(Vector2Int tile);
    }
}
