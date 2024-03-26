using UnityEngine;
using UnityEngine.InputSystem;

namespace DGJ24.Inputs {

	public class CameraLook : MonoBehaviour {

		[SerializeField] private float sensitivity = 0f;
		[SerializeField] private float verticalMinAngleClamp;
		[SerializeField] private float verticalMaxAngleClamp;
		[SerializeField] private float horizontalMinAngleClamp;
		[SerializeField] private float horizontalMaxAngleClamp;

		private Camera playerCam = null!;
		private float xRotation;
		private float yRotation;

		private Vector2 LookDelta { get; set; }

		private void Awake() {
			playerCam = GetComponent<Camera>();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update() {

			xRotation -= LookDelta.y * sensitivity;
			yRotation += LookDelta.x * sensitivity;
			xRotation = Mathf.Clamp(xRotation, verticalMinAngleClamp, verticalMaxAngleClamp);
			yRotation = Mathf.Clamp(yRotation, horizontalMinAngleClamp, horizontalMaxAngleClamp);
			playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

		}

		public void OnCameraLookInput(InputAction.CallbackContext ctx) {

			if (ctx.performed) {
				LookDelta = ctx.ReadValue<Vector2>();
			}

			if (ctx.canceled) {
				LookDelta = Vector2.zero;
			}

		}

	}

}