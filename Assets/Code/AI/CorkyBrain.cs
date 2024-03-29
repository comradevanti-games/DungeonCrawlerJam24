using System;
using System.Collections.Immutable;
using DGJ24.Actors;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class CorkyBrain : AIBrainBase
    {
        [SerializeField]
        private float stopDistance;

        [SerializeField]
        private AIBrainBase followPlayerBrain = null!;

        [SerializeField]
        private AIBrainBase attackBrain = null!;

        private ITileTransform tileTransform = null!;
        private IFloorPlan floorPlan = null!;
        private Transform playerTransform = null!;

        private bool IsBlockingCorridor()
        {
            return floorPlan.IsCorridor(tileTransform.Position);
        }

        private bool ShouldStop()
        {
            var distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer >= stopDistance)
                return false;

            return IsBlockingCorridor();
        }

        public override ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var attackAction = attackBrain.DetermineNextAction(ctx);
            if (attackAction is not NoOpActionRequest)
                return attackAction;

            if (ShouldStop())
                return new NoOpActionRequest(ctx.Actor);

            return followPlayerBrain.DetermineNextAction(ctx);
        }

        private void Awake()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            floorPlan = Singletons.Get<IFloorPlan>();
        }
    }
}
