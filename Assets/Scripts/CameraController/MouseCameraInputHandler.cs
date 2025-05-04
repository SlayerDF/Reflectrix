using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.CameraController
{
    public class MouseCameraInputHandler : MonoBehaviour, ICameraInputHandler
    {
        [SerializeField]
        private PanZoomSettigns panZoomSettigns;

        private Vector2? panInput;
        private float? zoomDelta;
        private bool isInitialized;
        private CameraController cameraController;
        private PlayerControls controls;

        /// <inheritdoc />
        public void Initialize(CameraController cameraController, PlayerControls controls)
        {
            if (isInitialized)
            {
                Debug.LogWarning("Camera input handler is already initialized.");
                return;
            }

            this.cameraController = cameraController;
            this.controls = controls;

            controls.Camera.MousePan.performed += OnCameraPanPerformed;
            controls.Camera.MousePan.canceled += OnCameraPanCanceled;

            controls.Camera.MouseZoom.performed += OnMouseZoomPerformed;
            controls.Camera.MouseZoom.canceled += OnMouseZoomCanceled;

            isInitialized = true;
        }

        private void OnDestroy()
        {
            if (controls != null)
            {
                controls.Camera.MousePan.performed -= OnCameraPanPerformed;
                controls.Camera.MousePan.canceled -= OnCameraPanCanceled;
                controls.Camera.MouseZoom.performed -= OnMouseZoomPerformed;
                controls.Camera.MouseZoom.canceled -= OnMouseZoomCanceled;
            }
        }

        private void OnMouseZoomCanceled(InputAction.CallbackContext context)
        {
            zoomDelta = null;
        }

        private void OnMouseZoomPerformed(InputAction.CallbackContext context)
        {
            var scroll = context.ReadValue<Vector2>();
            zoomDelta = Mathf.Sign(scroll.y);
        }

        private void OnCameraPanCanceled(InputAction.CallbackContext context)
        {
            panInput = null;
        }

        private void OnCameraPanPerformed(InputAction.CallbackContext context)
        {
            panInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (panInput.HasValue)
            {
                cameraController.ApplyPan(panInput.Value, panZoomSettigns.PanSpeed);
                panInput = null;
            }

            if (zoomDelta.HasValue)
            {
                cameraController.ApplyZoom(zoomDelta.Value, panZoomSettigns.ZoomSpeed, panZoomSettigns.MinZoom, panZoomSettigns.MaxZoom);
                zoomDelta = null;
            }
        }
    }
}