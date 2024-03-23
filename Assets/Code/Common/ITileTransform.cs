using UnityEngine;

namespace DGJ24
{
    /// <summary>
    /// Represents the transform of an object that lives in tile-space.
    /// </summary>
    public interface ITileTransform
    {
        /// <summary>
        /// The objects position.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// The direction the object is facing.
        /// </summary>
        public GridDirection Forward { get; set; }
    }
}
