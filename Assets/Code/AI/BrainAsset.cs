using UnityEngine;

namespace DGJ24.AI
{
    internal abstract class BrainAsset : ScriptableObject
    {
        public abstract IAIBrain Brain { get; }
    }
}
