using UnityEngine;

namespace DGJ24.TileSpace
{
    internal class TileSpaceGameObject : ITileSpaceEntity
    {
        public ITileTransform Transform { get; private init; } 

        public static TileSpaceGameObject? TryMakeFrom(GameObject gameObject)
        {
            var transform = gameObject.GetComponent<ITileTransform>();
            if (transform == null)
                return null;

            return new TileSpaceGameObject { Transform = transform };
        }
    }
}
