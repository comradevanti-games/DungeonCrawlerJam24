using UnityEngine;

namespace DGJ24.TileSpace
{
    /// <summary>
    /// Responsible for instantiating new tile-space entities.
    /// </summary>
    public interface ITileSpaceEntitySpawner
    {
        /// <summary>
        /// Instantiates a new tile-space entity.
        /// </summary>
        /// <param name="prefab">The entities prefab</param>
        /// <param name="tile">The tile on which to spawn the entity</param>
        /// <param name="forward">The direction the entity should face</param>
        /// <returns>The instantiated entity</returns>
        public GameObject Spawn(GameObject prefab, Vector2Int tile, CardinalDirection forward);
    }
}
