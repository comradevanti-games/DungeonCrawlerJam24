using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DGJ24.UI {

	public class LootDisplay : MonoBehaviour {

		[SerializeField] private TextMeshProUGUI lootTextMesh;

		public void OnLootCollected(int amount) {
			lootTextMesh.SetText($"x {amount}");
		}

	}

}