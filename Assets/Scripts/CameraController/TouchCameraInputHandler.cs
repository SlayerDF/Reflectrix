using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.CameraController
{
    public class TouchCameraInputHandler : MonoBehaviour, ICameraInputHandler
    {
        private Vector2 touch1Position;
        private Vector2 touch2Position;
        private bool isTouching1;
        private bool isTouching2;
        private float previousTouchDistance;
        private Vector2? panInput;
        private bool isInitialized;
        private CameraController cameraController;
        private PlayerControls controls;
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
            touch1Position = context.ReadValue<Vector2>();
            isTouching1 = true;
        }

        private void OnTouch2Canceled(InputAction.CallbackContext context)
        {
            isTouching2 = false;
        }

        private void OnTouch2Performed(InputAction.CallbackContext context)
        {
            touch2Position = context.ReadValue<Vector2>();
            isTouching2 = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            // Apply pan logic.
            if (isTouching1 && !isTouching2)
            {
                if (panInput.HasValue)
                {
                    cameraController.ApplyPan(panInput.Value);
                    panInput = null;
                }
            }

            // Apply zoom logic.
            if (isTouching1 && isTouching2)
            {
                float currentDistance = Vector2.Distance(touch1Position, touch2Position);

                if (previousDistance > 0f)
                {
                    float delta = previousDistance - currentDistance;
                    cameraController.ApplyZoom(Mathf.Sign(delta)); // scale factor
                }

                previousDistance = currentDistance;
            }
            else
            {
                previousDistance = 0f;
            }
        }
    }
}