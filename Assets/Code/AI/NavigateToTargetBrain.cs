using System.Linq;
using DGJ24.Actors;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class NavigateToTargetBrain : AIBrainBase
    {
        private IWalkableProvider walkableProvider = null!;
        private IPathfinder pathfinder = null!;
        private ITileTransform tileTransform = null!;
        private ITargetProvider targetProvider = null!;
        private Path? prevPath;

        private Path? TryFindPathToTarget()
        {
            var tile = targetProvider.CurrentTarget;
            if (tile == null)
                return null;

            return pathfinder.TryFindPath(tileTransform.Position, tile.Value);
        }

        private Path? ValidatePath(Path path)
        {
            path = path.SkipTo(tileTransform.Position);

            if (path.IsEmpty)
                return null;

            if (path.Targets.Last() != targetProvider.CurrentTarget)
                return null;

            return path;
        }

        private Path? TryUpdatePath(Path? currentPath)
        {
            var newPath = currentPath ?? TryFindPathToTarget();

            if (newPath == null)
                return null;

            return ValidatePath(newPath) ?? TryUpdatePath(null);
        }

        private ActionRequest Follow(GameObject actor, Path path)
        {
            var nextTile = path.Targets.First();
            if (!walkableProvider.IsWalkable(nextTile))
            {
                prevPath = null;
                return new NoOpActionRequest(actor);
            }

            var diff = nextTile - tileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
            {
                prevPath = null;
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
            var currentPath = TryUpdatePath(prevPath);
            prevPath = currentPath;

            return currentPath != null
                ? Follow(ctx.Actor, currentPath)
                : new NoOpActionRequest(ctx.Actor);
        }

        private void Awake()
        {
            pathfinder = Singletons.Get<IPathfinder>();
            walkableProvider = Singletons.Get<IWalkableProvider>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            targetProvider = gameObject.RequireComponent<ITargetProvider>();
        }
    }
}
