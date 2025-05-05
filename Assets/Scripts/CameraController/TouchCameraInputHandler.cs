using Reflectrix.RemoteConfig;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.CameraController
{
    public class TouchCameraInputHandler : MonoBehaviour, ICameraInputHandler
    {
        #region Serialized Fields

        [SerializeField]
        private PanZoomSettigns panZoomSettigns;

        #endregion

        private bool isInitialized;
        private bool isTouching1;
        private bool isTouching2;
        private CameraController cameraController;
        private float previousDistance;
        private PlayerControls controls;
        private Vector2? panInput;

        #region Event Functions

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            // Apply pan logic.
            if (isTouching1 && !isTouching2 && panInput.HasValue)
            {
                // TODO: remove usage of remote config
                cameraController.ApplyPan(panInput.Value, RemoteConfigFetcher.Config.panSpeed);
                isTouching1 = false;
                panInput = null;
            }

            // Apply zoom logic.
            if (isTouching1 && isTouching2)
            {
                // We have to read current finger positions every frame.
                var touch1 = Touchscreen.current.touches[0].position.ReadValue();
                var touch2 = Touchscreen.current.touches[1].position.ReadValue();

                var currentDistance = Vector2.Distance(touch1, touch2);

                if (previousDistance > 0f)
                {
                    var delta = currentDistance - previousDistance;
                    // TODO: remove usage of remote config
                    cameraController.ApplyZoom(delta * 0.01f, RemoteConfigFetcher.Config.zoomSpeed,
                        panZoomSettigns.MinZoom, panZoomSettigns.MaxZoom);
                }

                previousDistance = currentDistance;
                isTouching1 = false;
                isTouching2 = false;
            }
            else
            {
                previousDistance = 0f;
            }
        }

        private void OnDestroy()
        {
            if (controls != null)
            {
                controls.Camera.TouchDelta.performed -= OnTouchDelta;

                controls.Camera.PrimaryFingerPosition.performed -= OnTouch1Performed;
                controls.Camera.PrimaryFingerPosition.canceled -= OnTouch1Canceled;

                controls.Camera.SecondaryFingerPosition.performed -= OnTouch2Performed;
                controls.Camera.SecondaryFingerPosition.canceled -= OnTouch2Canceled;
            }
        }

        #endregion

        #region ICameraInputHandler Members

        /// <inheritdoc />
        public void Initialize(CameraController cameraController, PlayerControls controls)
        {
            if (isInitialized)
            {
                Debug.LogWarning("Camera input handler is already initialized.");
                return;
            }

            this.controls = controls;
            this.cameraController = cameraController;

            controls.Camera.TouchDelta.performed += OnTouchDelta;

            controls.Camera.PrimaryFingerPosition.performed += OnTouch1Performed;
            controls.Camera.PrimaryFingerPosition.canceled += OnTouch1Canceled;

            controls.Camera.SecondaryFingerPosition.performed += OnTouch2Performed;
            controls.Camera.SecondaryFingerPosition.canceled += OnTouch2Canceled;

            isInitialized = true;
        }

        #endregion

        private void OnTouchDelta(InputAction.CallbackContext context)
        {
            panInput = context.ReadValue<Vector2>();
        }

        private void OnTouch1Canceled(InputAction.CallbackContext context)
        {
            isTouching1 = false;
        }

        private void OnTouch1Performed(InputAction.CallbackContext context)
        {
            isTouching1 = true;
        }

        private void OnTouch2Canceled(InputAction.CallbackContext context)
        {
            isTouching2 = false;
        }

        private void OnTouch2Performed(InputAction.CallbackContext context)
        {
            isTouching2 = true;
        }
    }
}
