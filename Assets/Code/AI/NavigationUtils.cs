using System.Linq;
using DGJ24.Actors;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal static class NavigationUtils
    {
        public static ActionRequest TryDetermineNavigationActionFor(
            Path path,
            GameObject actor,
            ITileTransform tileTransform,
            IWalkableProvider walkableProvider
        )
        {
            var nextTile = path.Targets.First();
            if (!walkableProvider.IsWalkable(nextTile))
                return new NoOpActionRequest(actor);

            var diff = nextTile - tileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
                return new NoOpActionRequest(actor);

            var turnDir = TileSpaceMath.TryRotationTowards(
                tileTransform.Forward,
                dirToNextTile.Value
            );
            if (turnDir == null)
                return new MovementActionRequest(actor, dirToNextTile.Value, 0.5f);
            return new RotationActionRequest(actor, turnDir.Value, 0.5f);
        }
    }
}
