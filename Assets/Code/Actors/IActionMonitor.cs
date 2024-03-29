using System;
using System.Collections.Immutable;

namespace DGJ24.Actors
{
    /// <summary>
    /// Monitors all actors and raises an event when all have chosen an
    /// action-request to perform.
    /// </summary>
    public interface IActionMonitor
    {
        public record ActionBatchReadyArgs(IImmutableSet<ActionRequest> Batch);

        public record BeginMonitoringArgs;
        
        public event Action<BeginMonitoringArgs> BeginMonitoringActions;

        /// <summary>
        /// Event for when all actors have chosen an action-request.
        /// </summary>
        public event Action<ActionBatchReadyArgs> ActionBatchReady;
    }
}
