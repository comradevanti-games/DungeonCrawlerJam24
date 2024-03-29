using System;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.AI
{
    internal class TaggedObjectTargetProvider : MonoBehaviour, ITargetProvider
    {
        [SerializeField]
        private string targetTag = "";

        private Transform targetTransform = null!;

        public Vector2Int? CurrentTarget =>
            TileSpaceMath.PositionFromWorldSpace(targetTransform.position);

        private void Awake()
        {
            targetTransform = GameObject.FindWithTag(targetTag).transform;
        }
    }
}
