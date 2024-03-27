using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.AI
{
    internal abstract class BrainAsset : ScriptableObject, IAIBrain
    {
        public abstract ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx);
    }
}
