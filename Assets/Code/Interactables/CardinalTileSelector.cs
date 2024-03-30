using System.Collections.Generic;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class CardinalTileSelector : MonoBehaviour, IInteractionTileSelector
    {
        private ITileTransform tileTransform = null!;

        public IEnumerable<Vector2Int> Tiles =>
            TileSpaceMath.CardinalNeighborsOf(tileTransform.Position);

        private void Awake()
        {
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
