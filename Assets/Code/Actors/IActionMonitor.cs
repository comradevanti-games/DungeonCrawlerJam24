using System;
using System.Collections.Immutable;

namespace DGJ24.Actors {

	/// <summary>
	/// Monitors all actors and raises an event when all have chosen an
	/// action-request to perform.
	/// </summary>
	public interface IActionMonitor {

		public record ActionBatchReadyEvent(IImmutableSet<ActionRequest> Batch);

		/// <summary>
		/// Event for when all actors have chosen an action-request.
		/// </summary>
		public event Action<ActionBatchReadyEvent> ActionBatchReady;

		public event Action AllActionsExecuted;

	}

}