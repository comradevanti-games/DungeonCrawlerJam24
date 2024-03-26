using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.AI
{
    /// <summary>
    /// Contains logic for calculating AI actor actions.
    /// </summary>
    internal interface IAIBrain
    {
        public record ThinkContext(GameObject Actor);

        public ActionRequest DetermineNextAction(ThinkContext ctx);
    }
}
