using System;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Navigation
{
    internal class WalkableProvider : MonoBehaviour, IWalkableProvider
    {
        private IFloorPlan floorPlan = null!;
        private ITileSpaceEntityRepo tileSpaceEntityRepo = null!;

        public bool IsWalkable(Vector2Int tile)
        {
            return floorPlan.Contains(tile) && !tileSpaceEntityRepo.HasEntityAt(tile);
        }

        private void Awake()
        {
            floorPlan = Singletons.Get<IFloorPlan>();
            tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
        }
    }
}
