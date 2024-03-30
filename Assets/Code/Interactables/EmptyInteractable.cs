using System;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class EmptyInteractable : MonoBehaviour, IInteractable
    {
        public event Action<IInteractable.InteractedArgs>? Interacted;

        [SerializeField]
        private InteractionLayers layers;

        public InteractionLayers Layers => layers;

        public void HandleInteraction()
        {
            Interacted?.Invoke(new IInteractable.InteractedArgs());
        }
    }
}
