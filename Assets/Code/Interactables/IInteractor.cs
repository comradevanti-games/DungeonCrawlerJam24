using System.Collections.Generic;

namespace DGJ24.Interactables
{
    public interface IInteractor
    {
        public IEnumerable<IInteractable> PotentialInteractables { get; }

        public void TryInteract();
    }
}
