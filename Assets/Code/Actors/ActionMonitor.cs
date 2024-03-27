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

        private IActionValidator validator = null!;
        private IActorRepo actorRepo = null!;

        private void ValidateActorQueue(IActionRequestQueue queue)
        {
            while (true)
            {
                var next = queue.TryPeek();

                if (next == null)
                    return;
                if (validator.IsActionValid(next))
                    return;

                _ = queue.TryDequeue();
            }
        }

        private IImmutableSet<ActionRequest>? TryGetNextActionBatch(
            IReadOnlyCollection<GameObject> actors
        )
        {
            var queues = actors
                .Select(it => it.Require<IActionRequestQueue>())
                .ToImmutableHashSet();

            if (!queues.All(it => it.HasQueued))
                return null;

            queues.ForEach(ValidateActorQueue);

            if (!queues.All(it => it.HasQueued))
                return null;

            return queues.Select(it => it.TryDequeue()!).ToImmutableHashSet();
        }

        private async void MonitorActions()
        {
            BeginMonitoringActions?.Invoke();

            IImmutableSet<ActionRequest>? nextActionBatch = null;
            while (nextActionBatch == null)
            {
                await Task.Yield();
                nextActionBatch = TryGetNextActionBatch(actorRepo.Actors.ToImmutableHashSet());
            }

            ActionBatchReady?.Invoke(new IActionMonitor.ActionBatchReadyEvent(nextActionBatch));
        }

        private void Awake()
        {
            actorRepo = Singletons.Get<IActorRepo>();
            validator = Singletons.Get<IActionValidator>();
            Singletons.Get<IActorDirector>().AllActionsExecuted += () => MonitorActions();
        }

        private void Start()
        {
            MonitorActions();
        }
    }
}
