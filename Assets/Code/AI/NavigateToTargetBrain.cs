using System.Linq;
using DGJ24.Actors;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class NavigateToTargetBrain : AIBrainBase
    {
        private IPathNavigator pathNavigator = null!;
        private IWalkableProvider walkableProvider = null!;
        private ITileTransform tileTransform = null!;

        private ActionRequest Follow(GameObject actor, Path path)
        {
            var nextTile = path.Targets.First();
            if (!walkableProvider.IsWalkable(nextTile))
            {
                return new NoOpActionRequest(actor);
            }

            var diff = nextTile - tileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
            {
                pathNavigator.UpdatePath();
                return new NoOpActionRequest(actor);
            }

            var turnDir = TileSpaceMath.TryRotationTowards(
                tileTransform.Forward,
                dirToNextTile.Value
            );
            if (turnDir == null)
                return new MovementActionRequest(actor, dirToNextTile.Value, 0.5f);
            return new RotationActionRequest(actor, turnDir.Value, 0.5f);
        }

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var currentPath = pathNavigator.Path;
            return currentPath != null
                ? Follow(ctx.Actor, currentPath)
                : new NoOpActionRequest(ctx.Actor);
        }

        private void Awake()
        {
            walkableProvider = Singletons.Get<IWalkableProvider>();
            pathNavigator = gameObject.RequireComponent<IPathNavigator>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
