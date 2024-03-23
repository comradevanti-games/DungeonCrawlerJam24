using System;
using UnityEngine;

namespace DGJ24
{
    /// <summary>
    /// Access class for scene singletons
    /// </summary>
    public static class Singletons
    {
        private static GameObject Container =>
            GameObject.FindWithTag("Singletons")
            ?? throw new NullReferenceException("No singleton container found.");

        /// <summary>
        /// Attempts to get a singleton from the current scene.
        /// </summary>
        /// <typeparam name="T">The singletons type.</typeparam>
        /// <exception cref="NullReferenceException">If no singleton container is found.</exception>
        /// <returns>The singleton or null if not found.</returns>
        public static T? TryGet<T>()
            where T : class
        {
            var container = Container;
            return container.GetComponent<T>();
        }

        /// <summary>
        /// Attempts to get a singleton from the current scene.
        /// </summary>
        /// <typeparam name="T">The singletons type.</typeparam>
        /// <exception cref="NullReferenceException">If no singleton container is found.</exception>
        /// <exception cref="NullReferenceException">If the singleton is not found.</exception>
        /// <returns>The singleton.</returns>
        public static T Get<T>()
            where T : class
        {
            return TryGet<T>() ?? throw new NullReferenceException("Singleton not found.");
        }
    }
}
