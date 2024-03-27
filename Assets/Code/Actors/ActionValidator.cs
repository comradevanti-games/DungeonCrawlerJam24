using System;
using DGJ24.Map;
using DGJ24.TileSpace;
using UnityEngine;
using static DGJ24.TileSpace.TileSpaceMath;

namespace DGJ24.Actors
{
    internal class ActionValidator : MonoBehaviour, IActionValidator
    {
        private IWalkableService walkableService = null!;

        private bool CanDoMove(MovementActionRequest request)
        {
            var actorTransform = request.Actor.RequireComponent<ITileTransform>();
            var destinationTile = MoveByDirection(actorTransform.Position, request.Direction);
            return walkableService.IsWalkable(destinationTile);
        }

        public bool IsActionValid(ActionRequest request)
        {
            return request switch
            {
                NoOpActionRequest => true,
                RotationActionRequest => true,
                MovementActionRequest move => CanDoMove(move),
                // TODO: Check if interactable is in front of actor
                InteractionActionRequest => true,
                // TODO: Check charge
                ToolActionRequest => true,
                _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
            };
        }

        private void Awake()
        {
            walkableService = Singletons.Get<IWalkableService>();
        }
    }
}
