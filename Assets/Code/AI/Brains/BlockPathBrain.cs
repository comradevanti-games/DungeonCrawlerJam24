using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;
using DGJ24.Map;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class BlockPathBrain : AIBrainBase
    {
        [SerializeField]
        private float stopDistance;

        private IInteractor interactor = null!;
        private ITileTransform tileTransform = null!;
        private ITileTransform playerTransform = null!;
        private IFloorPlan floorPlan = null!;
        private IPathNavigator navigator = null!;
        private IWalkableProvider walkableProvider = null!;

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            // Attack player

            if (interactor.PotentialInteractables.Any())
                return new InteractionActionRequest(ctx.Actor);

            // Block path

            var tile = tileTransform.Position;
            var playerTile = playerTransform.Position;
            var distance = Vector2Int.Distance(tile, playerTile);

            var isCloseToPlayer = distance < stopDistance;
            var isInCorridor = floorPlan.IsCorridor(tile);
            var isBlockingPlayer = isCloseToPlayer && isInCorridor;
            if (isBlockingPlayer)
                return new NoOpActionRequest(ctx.Actor);

            // Move towards player

            navigator.Target = playerTile;
            var path = navigator.Path;
            if (path == null)
                return new NoOpActionRequest(ctx.Actor);

            var nextTile = path.Targets.First();
            if (!walkableProvider.IsWalkable(nextTile))
            {
                navigator.UpdatePath();
                return new NoOpActionRequest(ctx.Actor);
            }

            var diff = nextTile - tileTransform.Position;
            var dirToNextTile = TileSpaceMath.TryDirectionForVector(diff);
            if (dirToNextTile == null)
            {
                navigator.UpdatePath();
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
            interactor = gameObject.RequireComponent<IInteractor>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            navigator = gameObject.RequireComponent<IPathNavigator>();
            playerTransform = GameObject.FindWithTag("Player").RequireComponent<ITileTransform>();
            floorPlan = Singletons.Get<IFloorPlan>();
            walkableProvider = Singletons.Get<IWalkableProvider>();
        }
    }
}
