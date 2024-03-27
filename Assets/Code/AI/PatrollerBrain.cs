using System;
using System.Linq;
using DGJ24.Actors;
using DGJ24.Map;
using DGJ24.Pathfinding;
using DGJ24.TileSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DGJ24.AI
{
    internal class PatrollerBrain : MonoBehaviour, IAIBrain
    {
        private IPathfinder pathfinder = null!;
        private IWalkableService walkableService = null!;
        private ITileTransform tileTransform = null!;
        private Path? currentPath = null;

        private void TryUpdatePath()
        {
            var potentialTiles = walkableService.WalkableTiles;
            var index = Random.Range(0, potentialTiles.Count);
            var tile = potentialTiles.ElementAt(index);

            currentPath = pathfinder.TryFindPath(tileTransform.Position, tile);
        }

        public ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            if (currentPath == null || currentPath.IsEmpty)
                TryUpdatePath();

            if (currentPath == null || currentPath.IsEmpty)
                return new NoOpActionRequest(ctx.Actor);

            var nextTile = currentPath.Targets.First();
            var diff = nextTile - tileTransform.Position;
            var dir = TileSpaceMath.TryDirectionForVector(diff);
            if (dir == null) throw new Exception("Path included non-cardinal segment!");

            currentPath = currentPath.Step();
            
            return new MovementActionRequest(ctx.Actor, dir.Value, 0.1f);
        }

        private void Awake()
        {
            walkableService = Singletons.Get<IWalkableService>();
            pathfinder = Singletons.Get<IPathfinder>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
