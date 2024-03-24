namespace DGJ24.Actors
{
    /// <summary>
    /// An actor in the scene. Can execute actions
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// The actors action-request queue.
        /// </summary>
        public IActionRequestQueue ActionRequestQueue { get; }
    }
}
