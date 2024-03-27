namespace DGJ24.Actors
{
    internal class GameObjectActor : IActor
    {
        public IActionRequestQueue ActionRequestQueue { get; internal init; } = null!;
    }
}
