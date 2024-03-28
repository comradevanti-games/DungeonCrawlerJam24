using System;

namespace DGJ24.Actors {

	public interface IActionDirector {

		public event Action AllActionsExecuted;
		public event Action<int>? LootCollected;

	}

}