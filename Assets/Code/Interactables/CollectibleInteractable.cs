using System;
using DGJ24.Collectibles;
using UnityEngine;

namespace DGJ24.Interactables
{
    [RequireComponent(typeof(ICollectible))]
    internal class CollectibleInteractable : MonoBehaviour, IInteractable
    {
        public event Action<IInteractable.InteractedArgs>? Interacted;

        [SerializeField]
        private InteractionLayers layers;

        private ICollectible collectible = null!;

        public InteractionLayers Layers => layers;

        public void HandleInteraction()
        {
            collectible.Collect();
            Interacted?.Invoke(new IInteractable.InteractedArgs());
        }

        private void Awake()
        {
            collectible = gameObject.RequireComponent<ICollectible>();
        }
    }
}
