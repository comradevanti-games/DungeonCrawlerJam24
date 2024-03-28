using UnityEngine;

namespace DGJ24.Interactables {

	public interface IInteractable {

		public GameObject InteractableObject { get; set; }

		public InteractionLayer InteractionLayer { get; set; }

		public InteractionLayer[] InteractableLayers { get; }

		public bool CanInteract(InteractionLayer interactedLayer);

	}

}