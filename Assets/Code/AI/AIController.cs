using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.AI
{
    internal class AIController : MonoBehaviour
    {
        [SerializeField]
        private BrainAsset? brainAsset;

        private IActionRequestQueue requestQueue = null!;

        private IAIBrain Brain => brainAsset ? brainAsset!.Brain : new EmptyBrain();

        private void PlanNextAction()
        {
            var thinkContext = new IAIBrain.ThinkContext(gameObject);
            var nextAction = Brain.DetermineNextAction(thinkContext);
            requestQueue.TryEnqueue(nextAction);
        }

        private void Awake()
        {
            requestQueue = GetComponent<IActionRequestQueue>();
            Singletons.Get<IActionMonitor>().BeginMonitoringActions += () => PlanNextAction();
        }
    }
}
