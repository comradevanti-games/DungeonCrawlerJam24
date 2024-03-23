using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGJ24.Actors
{
    internal class ActorRepo : MonoBehaviour, IActorRepo
    {
        public IEnumerable<IActor> Actors { get; } = Enumerable.Empty<IActor>();
    }
}
