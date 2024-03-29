using DGJ24.Actors;

namespace DGJ24.AI
{
    internal record FallbackBrain(IAIBrain Primary, IAIBrain Fallback) : IAIBrain
    {
        public ActionRequest? DetermineNextAction(IAIBrain.ThinkContext ctx)
        {
            var action = Primary.DetermineNextAction(ctx);
            if (action is not NoOpActionRequest)
                return action;

            return Fallback.DetermineNextAction(ctx);
        }
    }
}
