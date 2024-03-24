using UnityEngine;

namespace DGJ24.Actors
{
    internal class GameObjectActor : IActor
    {
        public IActionRequestQueue ActionRequestQueue { get; private set; } = null!;

        public static GameObjectActor? TryMakeFrom(GameObject actorGameObject)
        {
            var queue = actorGameObject.GetComponent<IActionRequestQueue>();
            if (queue == null)
                return null;

            return new GameObjectActor { ActionRequestQueue = queue };
        }
    }
}
