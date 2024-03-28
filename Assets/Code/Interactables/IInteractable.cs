namespace DGJ24.Interactables
{
    public interface IInteractable
    {
        public InteractionLayers Layers { get; }

        public void HandleInteraction();
    }
}
