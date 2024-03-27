using UnityEngine;

namespace DGJ24.TileSpace
{
    public class TileTransform : MonoBehaviour, ITileTransform
    {
        public Vector2Int Position { get; set; }
        public GridDirection Forward { get; set; }
    }
}
