using UnityEngine;

namespace DGJ24.Navigation
{
    public interface IPathNavigator
    {
        public Vector2Int? Target { get; set; }

        public Path? Path { get; }

        public void UpdatePath();
    }
}
