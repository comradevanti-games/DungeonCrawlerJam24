using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.Actors
{
    public class ActionRequestQueue : MonoBehaviour, IActionRequestQueue
    {
        [SerializeField]
        private int maxRequestCount;

        private readonly Queue<ActionRequest> requests = new Queue<ActionRequest>();

        private int RequestCount => requests.Count;

        public bool HasQueued => RequestCount > 0;

        public ActionRequest? TryDequeue() => requests.TryDequeue(out var request) ? request : null;

        public bool TryEnqueue(ActionRequest request)
        {
            if (RequestCount == maxRequestCount)
                return false;

            // NOTE:
            //  Add additional enqueue rules here.
            //  I.e. Not allowed to queue interacts after other requests.

            requests.Enqueue(request);
            return true;
        }
    }
}
