using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;
using DGJ24.Map;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class CorkyBrain : AIBrainBase
    {
        [SerializeField]
        private float stopDistance;

        private IInteractor interactor = null!;
        private ITileTransform tileTransform = null!;
        private IFloorPlan floorPlan = null!;
        private IPathNavigator navigator = null!;
        private IWalkableProvider walkableProvider = null!;
        private INavigationTarget navigationTarget = null!;

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            // Attack player

            if (interactor.PotentialInteractables.Any())
                return new InteractionActionRequest(ctx.Actor);

            // Block path

            var tile = tileTransform.Position;
            var targetTile = navigationTarget.Tile;
            var distance = Vector2Int.Distance(tile, targetTile);

            var isCloseToPlayer = distance < stopDistance;
            var isInCorridor = floorPlan.IsCorridor(tile);
            var isBlockingPlayer = isCloseToPlayer && isInCorridor;
            if (isBlockingPlayer)
                return new NoOpActionRequest(ctx.Actor);

            // Move towards player

            navigator.Target = targetTile;
            var path = navigator.Path;
            if (path == null)
                return new NoOpActionRequest(ctx.Actor);

            var navigationRequest = NavigationUtils.TryDetermineNavigationActionFor(
                path,
                ctx.Actor,
                tileTransform,
                walkableProvider
            );
            if (navigationRequest is NoOpActionRequest)
                navigator.UpdatePath();

            return navigationRequest;
        }

        private void Awake()
        {
            interactor = gameObject.RequireComponent<IInteractor>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            navigator = gameObject.RequireComponent<IPathNavigator>();
            floorPlan = Singletons.Get<IFloorPlan>();
            walkableProvider = Singletons.Get<IWalkableProvider>();
            
           var playerTransform = GameObject.FindWithTag("Player").RequireComponent<ITileTransform>();
           navigationTarget = new TransformTarget(playerTransform);
        }
    }
}
