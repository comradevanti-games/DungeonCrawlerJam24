using System;

namespace DGJ24.Map
{
    /// <summary>
    /// Builds the generated map.
    /// </summary>
    public interface IMapBuilder
    {
        public record MapBuiltEvent;

        /// <summary>
        /// Raised when the builder completes building a map.
        /// </summary>
        public event Action<MapBuiltEvent> MapBuilt;
    }
}
