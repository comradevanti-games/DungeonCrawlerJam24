using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.AI
{
    internal class AIController : MonoBehaviour
    {
        private IActionRequestQueue requestQueue = null!;

        private IAIBrain brain = null!;

        private void PlanNextAction()
        {
            var thinkContext = new IAIBrain.ThinkContext(gameObject);
            var nextAction = brain.DetermineNextAction(thinkContext);
            requestQueue.TryEnqueue(nextAction);
        }

        private void Update()
        {
            if (!requestQueue.HasQueued)
                PlanNextAction();
        }

        private void Awake()
        {
            requestQueue = gameObject.RequireComponent<IActionRequestQueue>();
            brain = GetComponent<IAIBrain>() ?? new EmptyBrain();
        }
    }
}
