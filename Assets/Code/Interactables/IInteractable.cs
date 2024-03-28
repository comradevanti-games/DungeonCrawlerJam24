using UnityEngine;

namespace DGJ24.Interactables {

	public interface IInteractable {

		public GameObject InteractableObject { get; set; }

		public InteractionLayers InteractionLayers { get; set; }

		public InteractionLayers[] InteractableLayers { get; }

		public bool CanInteract(InteractionLayers interactedLayers);

	}

}