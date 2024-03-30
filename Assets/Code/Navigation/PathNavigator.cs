using System.Linq;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Navigation
{
    internal class PathNavigator : MonoBehaviour, IPathNavigator
    {
        private IPathfinder pathfinder = null!;
        private ITileTransform tileTransform = null!;
        private Vector2Int? target;

        public Vector2Int? Target
        {
            get => target;
            set
            {
                target = value;
                UpdatePath();
            }
        }

        public Path? Path { get; private set; }

        private Path? TryFindPathToTarget() =>
            Target != null ? pathfinder.TryFindPath(tileTransform.Position, Target.Value) : null;

        private Path? ValidatePath(Path path)
        {
            path = path.SkipTo(tileTransform.Position);

            if (path.IsEmpty)
                return null;

            if (path.Targets.Last() != Target)
                return null;

            return path;
        }

        public void UpdatePath()
        {
            if (Path == null)
                Path = TryFindPathToTarget();
            else
            {
                Path = ValidatePath(Path);
                if (Path == null)
                    UpdatePath();
            }
        }

        private void Awake()
        {
            pathfinder = Singletons.Get<IPathfinder>();
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
