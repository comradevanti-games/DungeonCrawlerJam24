using System.Collections.Generic;
using System.Linq;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class TileInteractor : MonoBehaviour, IInteractor
    {
        [SerializeField]
        private InteractionLayers targetLayers;

        private IInteractionTileSelector tileSelector = null!;
        private ITileSpaceEntityRepo tileSpaceEntityRepo = null!;

        public bool CanInteractWith(IInteractable interactable) =>
            (targetLayers & interactable.Layers) != 0;

        private IInteractable? TryGetValidInteractableAt(Vector2Int tile)
        {
            var entity = tileSpaceEntityRepo.TryGetEntityOn(tile);
            if (entity == null)
                return null;

            var interactable = entity.GetComponent<IInteractable>();
            if (interactable == null)
                return null;

            if (!CanInteractWith(interactable))
                return null;

            return interactable;
        }

        public IEnumerable<IInteractable> PotentialInteractables =>
            tileSelector.Tiles.Select(TryGetValidInteractableAt).FilterNull();

        public void TryInteract()
        {
            PotentialInteractables.ForEach(interactable =>
            {
                interactable.HandleInteraction();
            });
        }

        private void Awake()
        {
            tileSelector = gameObject.RequireComponent<IInteractionTileSelector>();
            tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
        }
    }
}
