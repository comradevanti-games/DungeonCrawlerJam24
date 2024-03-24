using System.Collections.Immutable;
using UnityEngine;

namespace DGJ24.Map
{
    internal record MapBlueprint(IImmutableSet<Vector2Int> FloorTiles)
    {
        public static readonly MapBlueprint empty = new MapBlueprint(
            ImmutableHashSet<Vector2Int>.Empty
        );

        public static MapBlueprint PlaceFloorAt(MapBlueprint blueprint, Vector2Int position)
        {
            return new MapBlueprint(blueprint.FloorTiles.Add(position));
        }
    }
}
