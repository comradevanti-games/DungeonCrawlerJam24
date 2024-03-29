using DGJ24.Actors;

namespace DGJ24.AI
{
    internal class EmptyBrain : IAIBrain
    {
        public ActionRequest DetermineNextAction(IAIBrain.ThinkContext ctx) =>
            new NoOpActionRequest(ctx.Actor);
    }
}
