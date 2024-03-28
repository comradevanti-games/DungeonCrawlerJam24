using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Actors {

	internal class ActorHealth : MonoBehaviour, IDamageable {

		public UnityEvent<int>? healthPointsChanged;
		public UnityEvent? actorDied;

		[SerializeField] private int baseHealth;

		private int healthPoints;

		private int HealthPoints {
			get => healthPoints;
			set {
				healthPoints = value;
				healthPointsChanged?.Invoke(HealthPoints);
			}
		}

		private void Awake() {
			HealthPoints = baseHealth;
		}

		public void Damage(int amount) {
			HealthPoints -= amount;

			if (HealthPoints <= 0) {
				Die();
			}

		}

		private void Die() {
			actorDied?.Invoke();
		}

	}

}