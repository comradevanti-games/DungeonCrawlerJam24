using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

namespace DGJ24.Actors {

	public class ActionMonitor : MonoBehaviour, IActionMonitor {

		public event Action<IActionMonitor.ActionBatchReadyEvent>? ActionBatchReady;
		private IActorRepo ActorRepo { get; set; }

		private void Start() {
			ActorRepo = Singletons.Get<IActorRepo>();
		}

		private void Update() {

			if (!ActorRepo.Actors.All(actor => actor.ActionRequestQueue.HasQueued)) return;

			ActionBatchReady?.Invoke(new IActionMonitor.ActionBatchReadyEvent(GetActionBatch(ActorRepo.Actors)));

		}

		private IImmutableSet<ActionRequest> GetActionBatch(IEnumerable<IActor> actors) {

			HashSet<ActionRequest> set = new();

			foreach (IActor? actor in actors) {
				set.Add(actor.ActionRequestQueue.TryDequeue());
			}

			return set.ToImmutableHashSet();

		}

	}

}