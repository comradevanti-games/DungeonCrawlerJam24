namespace DGJ24.Actors
{
    /// <summary>
    /// Actors can queue action-requests objects of this interface.
    /// </summary>
    public interface IActionRequestQueue
    {
        /// <summary>
        /// Checks if there is currently a request in the queue.
        /// </summary>
        public bool HasQueued { get; }

        /// <summary>
        /// Attempts to get the next request from the queue. This removes the
        /// request from the queue.
        /// </summary>
        /// <returns>The request or null if the queue is empty.</returns>
        public ActionRequest? TryDequeue();

        /// <summary>
        /// Attempts to enqueue an action-request into the queue.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Whether the request was enqueued.</returns>
        public bool TryEnqueue(ActionRequest request);
    }
}
