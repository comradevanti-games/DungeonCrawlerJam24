using System;

namespace DGJ24.Actors {

	public interface IActorTool {

		public event Action<int> RemainingCooldownChanged;

		public int Cooldown { get; }
		public int RemainingCooldown { get; }

		public void Use();

	}

}