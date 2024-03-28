using System;
using System.Linq;
using UnityEngine;

namespace DGJ24.Interactables {

	public class Interactable : MonoBehaviour, IInteractable {

		[SerializeField] private InteractionLayer interactionLayer;
		[SerializeField] private InteractionLayer[] interactableLayers = Array.Empty<InteractionLayer>();

		public GameObject InteractableObject { get; set; } = null!;
		public InteractionLayer InteractionLayer { get; set; } = InteractionLayer.None;
		public InteractionLayer[] InteractableLayers { get; private set; } = Array.Empty<InteractionLayer>();

		private void Awake() {
			InteractableObject = gameObject;
			InteractableLayers = interactableLayers;
			InteractionLayer = interactionLayer;
		}

		public bool CanInteract(InteractionLayer interactedLayer) {
			return InteractableLayers.Contains(interactedLayer);
		}

	}

}