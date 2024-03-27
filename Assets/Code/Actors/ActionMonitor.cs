using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DGJ24.Map;
using UnityEngine;

namespace DGJ24.Actors
{
    internal class ActionMonitor : MonoBehaviour, IActionMonitor
    {
        public event Action<IActionMonitor.ActionBatchReadyEvent>? ActionBatchReady;

        public event Action? BeginMonitoringActions;

        private IActorRepo actorRepo = null!;

        private IImmutableSet<ActionRequest> GetActionBatch(IEnumerable<IActor> actors)
        {
            HashSet<ActionRequest> set = new();

            foreach (IActor? actor in actors)
            {
                set.Add(actor.ActionRequestQueue.TryDequeue());
            }

            return set.ToImmutableHashSet();
        }

        private async void MonitorActions()
        {
            BeginMonitoringActions?.Invoke();

            while (true)
            {
                if (actorRepo.Actors.All(actor => actor.ActionRequestQueue.HasQueued))
                    break;
                await Task.Yield();
            }

            ActionBatchReady?.Invoke(
                new IActionMonitor.ActionBatchReadyEvent(GetActionBatch(actorRepo.Actors))
            );
        }

        private void Awake()
        {
            actorRepo = Singletons.Get<IActorRepo>();
            Singletons.Get<IActorDirector>().AllActionsExecuted += () => MonitorActions();
        }

        private void Start()
        {
            MonitorActions();
        }
    }
}
