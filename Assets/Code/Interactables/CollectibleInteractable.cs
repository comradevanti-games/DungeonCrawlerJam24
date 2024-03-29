using DGJ24.Collectibles;
using UnityEngine;

namespace DGJ24.Interactables
{
    [RequireComponent(typeof(ICollectible))]
    internal class CollectibleInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private InteractionLayers layers;

        private ICollectible collectible = null!;

        public InteractionLayers Layers => layers;

        public void HandleInteraction()
        {
            collectible.Collect();
        }

        private void Awake()
        {
            collectible = gameObject.RequireComponent<ICollectible>();
        }
    }
}
