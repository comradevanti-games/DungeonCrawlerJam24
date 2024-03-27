using System;
using System.Collections;
using UnityEngine;

namespace DGJ24
{
    /// <summary>
    /// Contains utility functions for lerping values over time.
    /// </summary>
    public static class LerpOverTime
    {
        public static IEnumerator Rotation(
            Transform rotateTransform,
            Quaternion origin,
            Quaternion targetRotation,
            float duration,
            Action onDone
        )
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                rotateTransform.rotation = Quaternion.Lerp(origin, targetRotation, progress);
                yield return null;
            }

            rotateTransform.rotation = targetRotation;
            onDone.Invoke();
        }

        public static IEnumerator Position(
            Transform moveTransform,
            Vector3 origin,
            Vector3 targetPosition,
            float duration,
            Action onDone
        )
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                moveTransform.position = Vector3.Lerp(origin, targetPosition, progress);
                yield return null;
            }

            moveTransform.position = targetPosition;
            onDone.Invoke();
        }
    }
}
