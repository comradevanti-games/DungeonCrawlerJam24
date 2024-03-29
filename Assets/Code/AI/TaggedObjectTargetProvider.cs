using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class TaggedObjectTargetProvider : TargetProviderBase
    {
        [SerializeField]
        private string targetTag = "";

        private Transform targetTransform = null!;

        public override Vector2Int? CurrentTarget =>
            TileSpaceMath.PositionFromWorldSpace(targetTransform.position);

        private void Awake()
        {
            targetTransform = GameObject.FindWithTag(targetTag).transform;
        }
    }
}
