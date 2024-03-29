using System;
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

        private IAIBrain internalBrain = null!;

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            return internalBrain.DetermineNextAction(ctx);
        }

        private void Awake()
        {
            var navigator = gameObject.RequireComponent<IPathNavigator>();
            var tileTransform = gameObject.RequireComponent<ITileTransform>();
            var interactor = gameObject.RequireComponent<IInteractor>();
            var playerTransform = GameObject
                .FindWithTag("Player")
                .RequireComponent<ITileTransform>();
            var walkableProvider = Singletons.Get<IWalkableProvider>();
            var floorPlan = Singletons.Get<IFloorPlan>();

            var playerTransformTarget = new TransformTargetProvider(playerTransform);
            var huntPlayerBrain = new NavigateToTargetBrain(
                navigator,
                walkableProvider,
                tileTransform
            );
            var attackBrain = new InteractBrain(interactor);
            var navigationBrain = new DistanceSwitchBrain(
                playerTransformTarget,
                tileTransform,
                (ctx, distance) =>
                {
                    if (distance < stopDistance && floorPlan.IsCorridor(tileTransform.Position))
                        return new NoOpActionRequest(ctx.Actor);

                    navigator.Target = playerTransform.Position;
                    return huntPlayerBrain.DetermineNextAction(ctx);
                },
                (_) => throw new Exception("Should always have target")
            );
            internalBrain = new FallbackBrain(attackBrain, navigationBrain);
        }
    }
}
