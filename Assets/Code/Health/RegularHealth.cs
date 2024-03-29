using DGJ24.Health;
using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Actors {

	internal class RegularHealth : MonoBehaviour, IHealth {

		public UnityEvent<int>? healthPointsChanged;
		public UnityEvent? actorDied;

		[SerializeField]
		private int baseHealth;

		private int value;

		public int Value {
			get => value;
			set {
				this.value = Mathf.Max(value, 0);
				healthPointsChanged?.Invoke(Value);

				if (Value == 0) Die();
			}
		}

		private void Awake() {
			Value = baseHealth;
		}

		private void Die() {
			actorDied?.Invoke();
		}

	}

}