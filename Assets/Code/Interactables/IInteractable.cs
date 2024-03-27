using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGJ24.Interactables {

	public interface IInteractable {

		public InteractionLayer InteractionLayer { get; set; }

		public InteractionLayer[] InteractableLayers { get; }

	}

}