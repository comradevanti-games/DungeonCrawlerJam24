using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    internal enum TileType
    {
        Floor,
        Wall
    }

    internal record MapBlueprint(IImmutableDictionary<Vector2Int, TileType> Tiles);
}
