using System;
using System.Collections.Generic;
using DGJ24.NPCs;
using UnityEngine;

namespace DGJ24.Actors
{
    internal class ActorRepo : MonoBehaviour, IActorRepo
    {
        [SerializeField]
        private GameObject[] initialActors = Array.Empty<GameObject>();

        private readonly IList<GameObject> actors = new List<GameObject>();

        public IEnumerable<GameObject> Actors => actors;

        private void AddActor(GameObject actor)
        {
            actors.Add(actor);
        }

        private void AddInitialActors()
        {
            initialActors.ForEach(AddActor);
        }

        private void Awake()
        {
            AddInitialActors();
            Singletons.Get<INpcSpawner>().NpcSpawned += args => AddActor(args.Npc);
        }
    }
}
