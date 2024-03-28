using System;
using System.Linq;
using DGJ24.Actors;
using DGJ24.Map;
using DGJ24.Navigation;
using DGJ24.TileSpace;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Random = UnityEngine.Random;

namespace DGJ24.AI
{
    internal class PatrollerBrain : MonoBehaviour, IAIBrain
    {
        private IPathfinder pathfinder = null!;
        private IFloorPlan floorPlan = null!;
        private ITileTransform tileTransform = null!;
        private Path? prevPath = null;

        private Path? TryFindRandomPath()
        {
            var potentialTiles = floorPlan.Tiles;
            var index = Random.Range(0, potentialTiles.Count);
            var tile = potentialTiles.ElementAt(index);

            return pathfinder.TryFindPath(tileTransform.Position, tile);
        }

        private Path? TryUpdatePath(Path? currentPath)
        {
            var newPath =
                currentPath != null
                    ? currentPath.SkipTo(tileTransform.Position)
                    : TryFindRandomPath();
            return newPath == null || newPath.IsEmpty ? null : newPath;
        }

        private ActionRequest Follow(GameObject actor, Path path)
        {
            var nextTile = path.Targets.First();
            var diff = nextTile - tileTransform.Position;
            var dir = TileSpaceMath.TryDirectionForVector(diff);
            if (dir == null)
            {
                prevPath = null;
                return new NoOpActionRequest(actor);
            }

            return new MovementActionRequest(actor, dir.Value, 0.5f);
        }

        public ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var currentPath = TryUpdatePath(prevPath);
            prevPath = currentPath;

            return currentPath != null
                ? Follow(ctx.Actor, currentPath)
                : new NoOpActionRequest(ctx.Actor);
        }

        private void Awake()
        {
            floorPlan = Singletons.Get<IFloorPlan>();
            pathfinder = Singletons.Get<IPathfinder>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
