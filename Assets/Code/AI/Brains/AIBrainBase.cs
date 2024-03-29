using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.AI
{
    internal abstract class AIBrainBase : MonoBehaviour, IAIBrain
    {
        public abstract ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx);
    }
}
