namespace DGJ24.Actors
{
    public interface IActionValidator
    {
        /// <summary>
        /// Checks whether an action can be executed based on the current
        /// game state.
        /// </summary>
        public bool IsActionValid(ActionRequest request);
    }
}
