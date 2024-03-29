using DGJ24.Health;
using UnityEngine;

namespace DGJ24.Interactables
{
    [RequireComponent(typeof(IHealth))]
    internal class DamageInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private InteractionLayers layers;

        private IHealth health = null!;

        public InteractionLayers Layers => layers;

        public void HandleInteraction()
        {
            health.Value--;
        }

        private void Awake()
        {
            health = gameObject.RequireComponent<IHealth>();
        }
    }
}
