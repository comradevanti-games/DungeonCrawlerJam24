using System;
using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class ProwlyBrain : AIBrainBase
    {
        private IInteractor interactor = null!;
        private IWalkableProvider walkableProvider = null!;
        private ITileTransform tileTransform = null!;
        private ITileTransform playerTransform = null!;
        private IPathfinder pathfinder = null!;
        private CardinalDirection targetDirection;

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            // Attack player

            if (interactor.PotentialInteractables.Any())
                return new InteractionActionRequest(ctx.Actor);

            // Rotate towards target

            var rotation = TileSpaceMath.TryRotationTowards(tileTransform.Forward, targetDirection);
            if (rotation != null)
                return new RotationActionRequest(ctx.Actor, rotation.Value, 0.5f);

            // Walk forward if possible

            var frontTile =
                tileTransform.Position + TileSpaceMath.VectorForDirection(targetDirection);
            var canWalkForward = walkableProvider.IsWalkable(frontTile);
            if (canWalkForward)
                return new MovementActionRequest(ctx.Actor, tileTransform.Forward, 0.5f);

            // Calc next direction

            var targetTile = playerTransform.Position;
            var path = pathfinder.TryFindPath(tileTransform.Position, targetTile);
            if (path == null)
                return new NoOpActionRequest(ctx.Actor);

            var nextTile = path.Targets.First();
            var nextDirection = TileSpaceMath.TryDirectionForVector(
                nextTile - tileTransform.Position
            );
            if (nextDirection == null)
                throw new Exception("Non-cardinal delta!");
            targetDirection = nextDirection.Value;

            return DetermineNextAction(ctx);
        }

        private void Awake()
        {
            interactor = gameObject.RequireComponent<IInteractor>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            walkableProvider = Singletons.Get<IWalkableProvider>();
            pathfinder = Singletons.Get<IPathfinder>();
            playerTransform = GameObject.FindWithTag("Player").RequireComponent<ITileTransform>();
        }
    }
}
