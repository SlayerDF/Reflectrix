using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.CameraController
{
    public class TouchCameraInputHandler : MonoBehaviour, ICameraInputHandler
    {
        private bool isInitialized;
        private CameraController cameraController;
        private PlayerControls controls;
        private bool isTouching1;
        private bool isTouching2;
        private Vector2? panInput;
        private float previousDistance = 0f;

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

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            // Apply pan logic.
            if (isTouching1 && !isTouching2 && panInput.HasValue)
            {
                cameraController.ApplyPan(panInput.Value);
                isTouching1 = false;
                panInput = null;
            }

            // Apply zoom logic.
            if (isTouching1 && isTouching2)
            {
                // We have to read current finger positions every frame.
                var touch1 = Touchscreen.current.touches[0].position.ReadValue();
                var touch2 = Touchscreen.current.touches[1].position.ReadValue();

                float currentDistance = Vector2.Distance(touch1, touch2);

                if (previousDistance > 0f)
                {
                    float delta = currentDistance - previousDistance;
                    cameraController.ApplyZoom(delta * 0.01f); // Negative = pinch in
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
    }
}