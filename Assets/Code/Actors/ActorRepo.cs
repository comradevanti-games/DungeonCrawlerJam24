using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.Actors
{
    internal class ActorRepo : MonoBehaviour, IActorRepo
    {
        [SerializeField]
        private GameObject[] initialActors = Array.Empty<GameObject>();

        private readonly IList<IActor> actors = new List<IActor>();

        public IEnumerable<IActor> Actors => actors;

        private bool TryAddActor(GameObject actorGameObject)
        {
            var actor = GameObjectActor.TryMakeFrom(actorGameObject);
            if (actor == null)
                return false;

            actors.Add(actor);
            return true;
        }

        private void AddInitialActors()
        {
            foreach (var initialActor in initialActors)
            {
                var wasSuccess = TryAddActor(initialActor);
                if (!wasSuccess)
                    throw new Exception($"{initialActor.name} was not a valid actor gameobject.");
            }
        }

        private void Awake()
        {
            AddInitialActors();
        }
    }
}
