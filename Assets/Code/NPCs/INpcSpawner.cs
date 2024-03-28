using System;
using UnityEngine;

namespace DGJ24.NPCs
{
    public interface INpcSpawner
    {
        public record NpcSpawnedEvent(GameObject Npc);

        public event Action<NpcSpawnedEvent> NpcSpawned;
    }
}
