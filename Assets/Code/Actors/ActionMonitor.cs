using System;
using UnityEngine;

namespace DGJ24.Actors {

	public class ActionMonitor : MonoBehaviour, IActionMonitor {

		public event Action<IActionMonitor.ActionBatchReadyEvent>? ActionBatchReady;

		private IActorRepo ActorRepo { get; set; }

		private void Start() {
			ActorRepo = Singletons.Get<IActorRepo>();
		}

	}

}