using UnityEngine;

namespace DGJ24.AI
{
    internal abstract class TargetProviderBase : MonoBehaviour, ITargetProvider
    {
        public abstract Vector2Int? CurrentTarget { get; }
    }
}
