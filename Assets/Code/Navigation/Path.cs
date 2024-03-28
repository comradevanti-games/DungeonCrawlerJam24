using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Navigation
{
    public record Path(IImmutableList<Vector2Int> Targets)
    {
        public bool IsEmpty => Targets.Count == 0;

        public static readonly Path Empty = new Path(ImmutableList<Vector2Int>.Empty);

        public Path SkipTo(Vector2Int tile)
        {
            var index = Targets.IndexOf(tile, 0, Targets.Count);
            return this with { Targets = Targets.RemoveRange(0, index + 1) };
        }
    };
}
