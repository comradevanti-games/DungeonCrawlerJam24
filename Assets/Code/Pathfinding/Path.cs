using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

namespace DGJ24.Pathfinding
{
    public record Path(IImmutableList<Vector2Int> Targets)
    {
        public bool IsEmpty => Targets.Count == 0;

        public static readonly Path Empty = new Path(ImmutableList<Vector2Int>.Empty);

        public Path Step()
        {
            return this with { Targets = Targets.RemoveAt(0) };
        }
    };
}
