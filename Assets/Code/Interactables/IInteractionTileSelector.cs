using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal interface IInteractionTileSelector
    {
        public IEnumerable<Vector2Int> Tiles { get; }
    }
}
