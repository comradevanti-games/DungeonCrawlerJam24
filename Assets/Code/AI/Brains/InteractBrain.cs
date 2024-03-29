using System.Linq;
using DGJ24.Actors;
using DGJ24.Interactables;

namespace DGJ24.AI
{
    internal record InteractBrain(IInteractor Interactor) : IAIBrain
    {
        public ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            return Interactor.PotentialInteractables.Any()
                ? new InteractionActionRequest(ctx.Actor)
                : new NoOpActionRequest(ctx.Actor);
        }
    }
}
