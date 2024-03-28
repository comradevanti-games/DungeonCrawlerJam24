using System.Collections.Generic;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class CircleTileSelector : MonoBehaviour, IInteractionTileSelector
    {
        private ITileTransform tileTransform = null!;

        public IEnumerable<Vector2Int> Tiles => TileSpaceMath.AllNeighborsOf(tileTransform.Position);

        private void Awake()
        {
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
