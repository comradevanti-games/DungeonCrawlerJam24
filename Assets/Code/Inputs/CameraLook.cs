using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	internal class CameraLook : MonoBehaviour {

		[SerializeField] private float sensitivity = 0f;
		[SerializeField] private float gamePadSens = 0f;
		[SerializeField] private float verticalMinAngleClamp;
		[SerializeField] private float verticalMaxAngleClamp;
		[SerializeField] private float horizontalMinAngleClamp;
		[SerializeField] private float horizontalMaxAngleClamp;

		private Camera playerCam = null!;
		private Coroutine moveRoutine = null!;
		private float xRotation;
		private float yRotation;

		private Vector2 LookDelta { get; set; }

		private void Awake() {
			playerCam = gameObject.RequireComponent<Camera>();
		}

		public void ResetView() {

			LookDelta = Vector2.zero;
			yRotation = 0;

			moveRoutine = StartCoroutine(LerpRotation(playerCam.transform, playerCam.transform.localRotation,
				Quaternion.Euler(xRotation, yRotation, 0f), 0.1f));

		}

		public void OnCameraLookInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {

				if (ctx.action.activeControl.device == Gamepad.current) {
					LookDelta = ctx.ReadValue<Vector2>() * gamePadSens;
				}
				else {
					LookDelta = ctx.ReadValue<Vector2>();
				}

				xRotation -= LookDelta.y * sensitivity;
				yRotation += LookDelta.x * sensitivity;
				xRotation = Mathf.Clamp(xRotation, verticalMinAngleClamp, verticalMaxAngleClamp);
				yRotation = Mathf.Clamp(yRotation, horizontalMinAngleClamp, horizontalMaxAngleClamp);
				playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

			}

			if (ctx.canceled) {
				LookDelta = Vector2.zero;
			}

		}

		private static IEnumerator LerpRotation(Transform t, Quaternion origin, Quaternion targetRotation, float duration) {

			float startTime = Time.time;
			float endTime = startTime + duration;

			while (Time.time < endTime) {
				float progress = (Time.time - startTime) / duration;
				t.localRotation = Quaternion.Lerp(origin, targetRotation, progress);
				yield return null;
			}

			t.localRotation = targetRotation;

		}

	}

}