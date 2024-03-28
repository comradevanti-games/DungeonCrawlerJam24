using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Interactables
{
    internal class FrontInteractor : MonoBehaviour, IInteractor
    {
        [SerializeField]
        private InteractionLayers targetLayers;

        private ITileTransform tileTransform = null!;
        private ITileSpaceEntityRepo tileSpaceEntityRepo = null!;

        public void TryInteract()
        {
            var interactionTile =
                tileTransform.Position + TileSpaceMath.VectorForDirection(tileTransform.Forward);
            var entity = tileSpaceEntityRepo.TryGetEntityOn(interactionTile);
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

        private void Awake()
        {
            tileTransform = gameObject.RequireComponent<ITileTransform>();
            tileSpaceEntityRepo = Singletons.Get<ITileSpaceEntityRepo>();
        }
    }
}
