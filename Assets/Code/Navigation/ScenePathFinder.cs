using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AStarNavigator;
using AStarNavigator.Algorithms;
using AStarNavigator.Providers;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Navigation
{
    internal class ScenePathFinder
        : MonoBehaviour,
            IPathfinder,
            IBlockedProvider,
            INeighborProvider,
            IDistanceAlgorithm
    {
        private IWalkableProvider walkableProvider = null!;
        private TileNavigator navigator = null!;

        public bool IsBlocked(Tile coord) => !walkableProvider.IsWalkable(coord.ToV2());

        public IEnumerable<Tile> GetNeighbors(Tile tile) =>
            TileSpaceMath
                .CardinalNeighborsOf(tile.ToV2())
                .Where(walkableProvider.IsWalkable)
                .Select(it => it.ToTile());

        public double Calculate(Tile from, Tile to) => 1;

        public Path? TryFindPath(Vector2Int from, Vector2Int to)
        {
            var pathTiles = navigator.Navigate(from.ToTile(), to.ToTile())?.Select(it => it.ToV2());
            if (pathTiles == null)
                return null;
            return new Path(pathTiles.ToImmutableList());
        }

        private void Awake()
        {
            navigator = new TileNavigator(this, this, this, this);
            walkableProvider = Singletons.Get<IWalkableProvider>();
        }
    }
}
