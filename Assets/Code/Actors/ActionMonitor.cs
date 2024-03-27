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

        private void ValidateActorQueue(IActor actor)
        {
            while (true)
            {
                var next = actor.ActionRequestQueue.TryPeek();

                if (next == null)
                    return;
                if (validator.IsActionValid(next))
                    return;

                
                _ = actor.ActionRequestQueue.TryDequeue();
            }
        }

        private IImmutableSet<ActionRequest>? TryGetNextActionBatch(
            IReadOnlyCollection<IActor> actors
        )
        {
            if (!actors.All(it => it.ActionRequestQueue.HasQueued))
                return null;

            
            actors.ForEach(ValidateActorQueue);

            if (!actors.All(it => it.ActionRequestQueue.HasQueued))
                return null;

            
            return actors.Select(it => it.ActionRequestQueue.TryDequeue()!).ToImmutableHashSet();
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
