using System;

namespace DGJ24.Interactables
{
    [Flags]
    public enum InteractionLayers
    {
        None = 0,
        Player = 1,
        Enemy = 2,
        Loot = 4
    }
}
