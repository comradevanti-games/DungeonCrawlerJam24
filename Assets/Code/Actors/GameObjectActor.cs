using UnityEngine;

namespace DGJ24.Actors
{
    internal class GameObjectActor : IActor
    {
        public static GameObjectActor? TryMakeFrom(GameObject actorGameObject)
        {
            return new GameObjectActor();
        }
    }
}
