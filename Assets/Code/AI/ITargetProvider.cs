using UnityEngine;

namespace DGJ24.AI
{
    public interface ITargetProvider
    {
        public Vector2Int? CurrentTarget { get; }
    }
}
