using System;

namespace DGJ24.Map
{
    /// <summary>
    /// Builds the generated map.
    /// </summary>
    internal interface IMapBuilder
    {
        public record MapBuiltEvent(MapBlueprint BuiltBlueprint);

        /// <summary>
        /// Raised when the builder completes building a map.
        /// </summary>
        public event Action<MapBuiltEvent> MapBuilt;
    }
}
