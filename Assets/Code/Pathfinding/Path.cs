using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Pathfinding
{
    public record Path(IImmutableList<Vector2Int> Targets)
    {
        public static readonly Path Empty = new Path(ImmutableList<Vector2Int>.Empty);
    };
}
