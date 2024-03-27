using System;
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
        public IEnumerable<ITileSpaceEntity> All { get; }

        /// <summary>
        /// Attempt to add a game-object to this repo.
        /// </summary>
        /// <param name="entityGameObject">The game-object to add.</param>
        /// <returns>Whether the game-object could be added.</returns>
        public bool TryAdd(GameObject entityGameObject);

        public void AddOrThrow(GameObject entityGameObject)
        {
            var success = TryAdd(entityGameObject);
            if (!success)
                throw new Exception($"{entityGameObject.name} is not a tile-space entity");
        }
    }
}
