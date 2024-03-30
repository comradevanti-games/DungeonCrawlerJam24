using System;
using DGJ24.Actors;
using UnityEngine;

namespace DGJ24.Game.UI {

	public class CooldownDisplay : MonoBehaviour {

		private const float InitialYPos = -3f;
		private const float AvailableHeight = 114f;

		[SerializeField] private GameObject cooldownPiecePrefab = null!;
		[SerializeField] private GameObject playerPrefab = null!;

		private IActorTool ActiveActorTool { get; set; } = null!;

		private GameObject[] CooldownPieces { get; set; } = Array.Empty<GameObject>();

		private void Awake() {
			ActiveActorTool = playerPrefab.GetComponentInChildren<IActorTool>();
			ActiveActorTool.RemainingCooldownChanged += OnRemainingCooldownChanged;
		}

		private void Start() {

			float pieceHeight = AvailableHeight / ActiveActorTool.Cooldown;
			CooldownPieces = new GameObject[ActiveActorTool.Cooldown];

			for (int i = 0; i < ActiveActorTool.Cooldown; i++) {

				RectTransform? piece = Instantiate(cooldownPiecePrefab, transform).GetComponent<RectTransform>();
				piece.anchoredPosition = new Vector2(0, (InitialYPos - AvailableHeight + pieceHeight) + (pieceHeight * i));
				piece.sizeDelta = new Vector2(piece.sizeDelta.x, pieceHeight);
				CooldownPieces[i] = piece.gameObject;

			}

		}

		private void OnRemainingCooldownChanged(int remaining) {

			int c = (ActiveActorTool.Cooldown - 1) - remaining;

			for (int i = 0; i < CooldownPieces.Length; i++) {
				CooldownPieces[i].SetActive(i <= c);
			}

		}

	}

}