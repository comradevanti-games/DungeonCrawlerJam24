using System.Linq;
using DGJ24.Actors;
using DGJ24.Navigation;
using DGJ24.TileSpace;

namespace DGJ24.AI
{
    internal record NavigateToTargetBrain(
        IPathNavigator PathNavigator,
        IWalkableProvider WalkableProvider,
        ITileTransform TileTransform
    ) : IAIBrain
    {
        public ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var path = PathNavigator.Path;
            if (path == null)
                return new NoOpActionRequest(ctx.Actor);

            var nextTile = path.Targets.First();
            if (!WalkableProvider.IsWalkable(nextTile))
            {
                return new NoOpActionRequest(ctx.Actor);
            }

            var diff = nextTile - TileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
            {
                PathNavigator.UpdatePath();
                return new NoOpActionRequest(ctx.Actor);
            }

            var turnDir = TileSpaceMath.TryRotationTowards(
                TileTransform.Forward,
                dirToNextTile.Value
            );
            if (turnDir == null)
                return new MovementActionRequest(ctx.Actor, dirToNextTile.Value, 0.5f);
            return new RotationActionRequest(ctx.Actor, turnDir.Value, 0.5f);
        }
    }
}
