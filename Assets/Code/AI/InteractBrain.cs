using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;

namespace DGJ24.AI
{
    internal class InteractBrain : AIBrainBase
    {
        private IInteractor interactor = null!;

        public override ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            return interactor.PotentialInteractables.Any()
                ? new InteractionActionRequest(ctx.Actor)
                : new NoOpActionRequest(ctx.Actor);
        }

        private void Awake()
        {
            interactor = gameObject.RequireComponent<IInteractor>();
        }
    }
}
