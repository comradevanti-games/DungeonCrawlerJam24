using System;
using UnityEngine;

namespace DGJ24
{
    public static class RequireExt
    {
        public static T RequireComponent<T>(this GameObject gameObject)
            where T : class
        {
            return gameObject.GetComponent<T>()
                ?? throw new Exception(
                    $"{gameObject.name} was required to have component matching {typeof(T).Name}, but it did not!"
                );
        }
    }
}
