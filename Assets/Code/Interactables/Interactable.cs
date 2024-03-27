using System;
using UnityEngine;

namespace DGJ24.Interactables {

	public class Interactable : MonoBehaviour, IInteractable {

		[SerializeField] private InteractionLayer interactionLayer;
		[SerializeField] private InteractionLayer[] interactableLayers;

		public InteractionLayer InteractionLayer { get; set; } = InteractionLayer.None;
		public InteractionLayer[] InteractableLayers { get; private set; } = Array.Empty<InteractionLayer>();

		private void Awake() {
			InteractableLayers = interactableLayers;
			InteractionLayer = interactionLayer;
		}

	}

}