using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;
using DGJ24.Map;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class ShortyBrain : AIBrainBase
    {
        [SerializeField]
        private int huntRoundCount;

        [SerializeField]
        private int scatterRoundCount;

        private int roundIndex;
        private IInteractor interactor = null!;
        private ITileTransform tileTransform = null!;
        private INavigationTarget playerTarget = null!;
        private IPathNavigator navigator = null!;
        private IWalkableProvider walkableProvider = null!;
        private IFloorPlan floorPlan = null!;
        private INavigationTarget navigationTarget = null!;

        private INavigationTarget RandomTileTarget()
        {
            var index = Random.Range(0, floorPlan.Tiles.Count);
            var tile = floorPlan.Tiles.ElementAt(index);
            return new ConstantTarget(tile);
        }

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            // Attack player

            if (interactor.PotentialInteractables.Any())
                return new InteractionActionRequest(ctx.Actor);

            // Move towards target

            var targetTile = navigationTarget.Tile;
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

        private void ProgressRoundIndex()
        {
            roundIndex = (int)Mathf.Repeat(roundIndex + 1, huntRoundCount + scatterRoundCount - 1);
            if (roundIndex == 0)
                navigationTarget = playerTarget;
            else if (roundIndex == huntRoundCount)
                navigationTarget = RandomTileTarget();
        }

        private void Awake()
        {
            interactor = gameObject.RequireComponent<IInteractor>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            navigator = gameObject.RequireComponent<IPathNavigator>();
            walkableProvider = Singletons.Get<IWalkableProvider>();
            floorPlan = Singletons.Get<IFloorPlan>();
            Singletons.Get<IActionDirector>().AllActionsExecuted += () => ProgressRoundIndex();

            var playerTransform = GameObject
                .FindWithTag("Player")
                .RequireComponent<ITileTransform>();
            playerTarget = new TransformTarget(playerTransform);
            navigationTarget = playerTarget;
        }
    }
}
