using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    internal record MapBlueprint(IImmutableSet<Vector2Int> FloorTiles);
}
