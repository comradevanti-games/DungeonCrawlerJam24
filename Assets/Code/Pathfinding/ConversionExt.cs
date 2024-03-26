using AStarNavigator;
using UnityEngine;

namespace DGJ24.Pathfinding
{
    internal static class ConversionExt
    {
        public static Vector2Int ToV2(this Tile x) => new Vector2Int((int)x.X, (int)x.Y);

        public static Tile ToTile(this Vector2Int x) => new Tile(x.x, x.y);
    }
}
