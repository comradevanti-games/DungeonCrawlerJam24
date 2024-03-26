using UnityEngine;
using UnityEngine.Events;

namespace DGJ24.Actors {

	public class ActorHealth : MonoBehaviour, IDamageable {

		public UnityEvent<int>? healthPointsChanged;

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
		}

	}

}