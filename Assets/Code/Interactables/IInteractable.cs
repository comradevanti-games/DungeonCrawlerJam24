using System;

namespace DGJ24.Interactables
{
    public interface IInteractable
    {
        public record InteractedArgs;

        public event Action<InteractedArgs> Interacted;

        public InteractionLayers Layers { get; }

        public void HandleInteraction();
    }
}
