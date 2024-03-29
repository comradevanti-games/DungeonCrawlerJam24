using DGJ24.Health;
using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Interactables {

	[RequireComponent(typeof(IHealth))]
	internal class DamageInteractable : MonoBehaviour, IInteractable {

		[SerializeField]
		private InteractionLayers layers;

		[SerializeField] private UnityEvent? damageTaken;

		private IHealth health = null!;

		public InteractionLayers Layers => layers;

		public void HandleInteraction() {
			health.Value--;
			damageTaken?.Invoke();
		}

		private void Awake() {
			health = gameObject.RequireComponent<IHealth>();
		}

	}

}