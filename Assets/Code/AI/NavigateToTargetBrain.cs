using System.Linq;
using DGJ24.Actors;
using DGJ24.Navigation;
using DGJ24.TileSpace;

namespace DGJ24.AI
{
    internal class NavigateToTargetBrain : AIBrainBase
    {
        private IPathNavigator pathNavigator = null!;
        private IWalkableProvider walkableProvider = null!;
        private ITileTransform tileTransform = null!;

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var path = pathNavigator.Path;
            if (path == null)
                return new NoOpActionRequest(ctx.Actor);

            var nextTile = path.Targets.First();
            if (!walkableProvider.IsWalkable(nextTile))
            {
                return new NoOpActionRequest(ctx.Actor);
            }

            var diff = nextTile - tileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
            {
                pathNavigator.UpdatePath();
                return new NoOpActionRequest(ctx.Actor);
            }

            var turnDir = TileSpaceMath.TryRotationTowards(
                tileTransform.Forward,
                dirToNextTile.Value
            );
            if (turnDir == null)
                return new MovementActionRequest(ctx.Actor, dirToNextTile.Value, 0.5f);
            return new RotationActionRequest(ctx.Actor, turnDir.Value, 0.5f);
        }

        private void Awake()
        {
            walkableProvider = Singletons.Get<IWalkableProvider>();
            pathNavigator = gameObject.RequireComponent<IPathNavigator>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
