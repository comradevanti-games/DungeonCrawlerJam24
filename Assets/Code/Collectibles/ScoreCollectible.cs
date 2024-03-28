using DGJ24.Score;
using UnityEngine;

namespace DGJ24.Collectibles
{
    internal class ScoreCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField]
        private int value;

        public void Collect()
        {
            Singletons.Get<IScoreTracker>().Add(value);

            Destroy(gameObject);
        }
    }
}
