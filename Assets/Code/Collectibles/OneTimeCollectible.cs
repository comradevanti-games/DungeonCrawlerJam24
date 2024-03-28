using UnityEngine;

namespace DGJ24.Collectibles
{
    internal class OneTimeCollectible : MonoBehaviour, ICollectible
    {
        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}
