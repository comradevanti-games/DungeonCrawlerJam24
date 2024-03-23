using System.Collections.Generic;

namespace DGJ24.Actors
{
    /// <summary>
    /// Keeps track of all actors which are currently in the scene
    /// </summary>
    public interface IActorRepo
    {
        /// <summary>
        /// All actors in the current scene.
        /// </summary>
        public IEnumerable<IActor> Actors { get; }
    }
}
