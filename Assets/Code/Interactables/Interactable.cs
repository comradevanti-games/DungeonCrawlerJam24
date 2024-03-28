using System;
using System.Linq;
using UnityEngine;

namespace DGJ24.Interactables {

	internal class Interactable : MonoBehaviour, IInteractable {

		[SerializeField] private InteractionLayers interactionLayers;
		[SerializeField] private InteractionLayers[] interactableLayers = Array.Empty<InteractionLayers>();

		public GameObject InteractableObject { get; set; } = null!;
		public InteractionLayers InteractionLayers { get; set; } = InteractionLayers.None;
		public InteractionLayers[] InteractableLayers { get; private set; } = Array.Empty<InteractionLayers>();

		private void Awake() {
			InteractableObject = gameObject;
			InteractableLayers = interactableLayers;
			InteractionLayers = interactionLayers;
		}

		public bool CanInteract(InteractionLayers interactedLayers) {
			return InteractableLayers.Contains(interactedLayers);
		}

	}

}