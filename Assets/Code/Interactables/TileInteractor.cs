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

        private void TryInteractAt(Vector2Int tile)
        {
            var entity = tileSpaceEntityRepo.TryGetEntityOn(tile);
            if (entity == null)
                return;

            var interactable = entity.GetComponent<IInteractable>();
            if (interactable == null)
                return;

            var canInteract = (targetLayers & interactable.Layers) != 0;
            if (!canInteract)
                return;

            interactable.HandleInteraction();
        }

        public void TryInteract()
        {
            var interactionTiles = tileSelector.Tiles;
            interactionTiles.ForEach(TryInteractAt);
        }

        private void Awake()
        {
            tileSelector = gameObject.RequireComponent<IInteractionTileSelector>();
            tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
        }
    }
}
