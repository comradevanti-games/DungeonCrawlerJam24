namespace DGJ24.Actors {

	public interface IActorTool {

		public int Cooldown { get; }
		public int RemainingCooldown { get; }

		public void Use();

	}

}