namespace DGJ24.TileSpace
{
    /// <summary>
    /// An entity which exists in tile-space.
    /// </summary>
    public interface ITileSpaceEntity
    {
        /// <summary>
        /// The entities transform
        /// </summary>
        public ITileTransform Transform { get; }
    }
}
