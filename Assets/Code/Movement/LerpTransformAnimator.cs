using System.Collections.Generic;
using System.Threading.Tasks;
using DGJ24.TileSpace;
using UnityEngine;

namespace DGJ24.Movement
{
    internal class LerpTransformAnimator : MonoBehaviour, ITransformAnimator
    {
        [SerializeField]
        private float syncSeconds;

        private ITileTransform tileTransform = null!;

        private Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        private async IAsyncEnumerable<float> LerpT()
        {
            var t = 0f;
            yield return t;

            while (t < 1)
            {
                await Task.Yield();
                t = Mathf.MoveTowards(t, 1, Time.deltaTime / syncSeconds);
                yield return t;
            }
        }

        public async Task SyncPosition()
        {
            var start = Position;
            var target = TileSpaceMath.PositionToWorldSpace(tileTransform.Position);
            await foreach (var t in LerpT())
            {
                if (destroyCancellationToken.IsCancellationRequested)
                    return;
                Position = Vector3.Lerp(start, target, t);
            }
        }

        public async Task SyncRotation()
        {
            var start = Rotation;
            var target = Quaternion.LookRotation(
                TileSpaceMath.DirectionToWorldSpace(tileTransform.Forward)
            );
            await foreach (var t in LerpT())
            {
                if (destroyCancellationToken.IsCancellationRequested)
                    return;
                Rotation = Quaternion.Lerp(start, target, t);
            }
        }

        private void Awake()
        {
            tileTransform = gameObject.RequireComponent<ITileTransform>();
        }
    }
}
