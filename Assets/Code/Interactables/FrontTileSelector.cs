using System.Collections.Generic;
using System.Linq;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class FrontTileSelector : MonoBehaviour, IInteractionTileSelector
    {
        private ITileTransform tileTransform = null!;

        public IEnumerable<Vector2Int> Tiles
        {
            get
            {
                yield return tileTransform.Position
                    + TileSpaceMath.VectorForDirection(tileTransform.Forward);
            }
        }

        private void Awake()
        {
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
