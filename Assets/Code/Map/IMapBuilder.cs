using System;
using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    /// <summary>
    /// Builds the generated map.
    /// </summary>
    internal interface IMapBuilder
    {
        public record MapBuiltEvent(IImmutableSet<Vector2Int> FloorTiles);

        /// <summary>
        /// Raised when the builder completes building a map.
        /// </summary>
        public event Action<MapBuiltEvent> MapBuilt;
    }
}
