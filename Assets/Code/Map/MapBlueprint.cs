using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    public enum TileType
    {
        Floor,
        Wall
    }

    public record MapBlueprint(IImmutableDictionary<Vector2Int, TileType> Tiles);
}
