using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.TileSpace
{
    /// <summary>
    /// Grants access to tile-space entities
    /// </summary>
    public interface ITileSpaceEntityRepo
    {
        /// <summary>
        /// All tile-space entities in the scene.
        /// </summary>
        public IEnumerable<GameObject> All { get; }

        /// <summary>
        /// Adds a game-object to this repo. It is your responsibility to make
        /// sure that this object is a valid tile-space entity.
        /// </summary>
        /// <param name="entity">The game-object to add.</param>
        public void Add(GameObject entity);



        public bool EntityIsBlocking(Vector2Int tile);

    }
}
