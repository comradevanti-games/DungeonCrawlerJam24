using System;
using DGJ24.Health;
using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Interactables
{
    [RequireComponent(typeof(IHealth))]
    internal class DamageInteractable : MonoBehaviour, IInteractable
    {
        public event Action<IInteractable.InteractedArgs>? Interacted;

        [SerializeField]
        private InteractionLayers layers;

        [SerializeField]
        private UnityEvent? damageTaken;

        private IHealth health = null!;

        public InteractionLayers Layers => layers;

        public void HandleInteraction()
        {
            health.Value--;
            damageTaken?.Invoke();
            Interacted?.Invoke(new IInteractable.InteractedArgs());
        }

        private void Awake()
        {
            health = gameObject.RequireComponent<IHealth>();
        }
    }
}
