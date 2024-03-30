using System.Collections.Generic;
using System.Threading.Tasks;
using DGJ24.TileSpace;
using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Movement
{
    internal class LerpTransformAnimator : MonoBehaviour, ITransformAnimator
    {
        [SerializeField]
        private float syncSeconds;

        [SerializeField]
        private UnityEvent animationStarted = new UnityEvent();

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
            animationStarted.Invoke();

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
            animationStarted.Invoke();

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
