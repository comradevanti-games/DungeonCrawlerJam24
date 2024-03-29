using System;
using DGJ24.Actors;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal record DistanceSwitchBrain(
        ITargetTileProvider TargetTileProvider,
        ITileTransform Transform,
        Func<IAIBrain.ThinkContext, float, ActionRequest?> SwitchFunc,
        Func<IAIBrain.ThinkContext, ActionRequest?> NoTargetFunc
    ) : IAIBrain
    {
        public ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var target = TargetTileProvider.Tile;
            if (target == null)
                return NoTargetFunc(ctx);

            var distance = Vector2Int.Distance(target.Value, Transform.Position);
            return SwitchFunc(ctx, distance);
        }
    };
}
