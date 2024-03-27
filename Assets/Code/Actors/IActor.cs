using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Actors
{
    /// <summary>
    /// An actor in the scene. Can execute actions
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// The actors action-request queue.
        /// </summary>
        public IActionRequestQueue ActionRequestQueue { get; }

        public static IActor? TryMakeFrom(GameObject actorGameObject)
        {
            var queue = actorGameObject.GetComponent<IActionRequestQueue>();
            if (queue == null)
                return null;

            return new GameObjectActor { ActionRequestQueue = queue };
        }
    }
}
