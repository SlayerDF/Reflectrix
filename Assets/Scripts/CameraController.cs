using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix
{
    public class CameraController : MonoBehaviour
    {
        [Header("Pan Settings")]
        [SerializeField]
        private float panSpeed = 1f;

        [SerializeField]
        private float zoomSpeed = 0.1f;

        [SerializeField]
        private float minZoom = 5f;

        [SerializeField]
        private float maxZoom = 15f;

        [SerializeField]
        private Camera cameraObject;

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private InputManager inputManager;

        private Vector2? panInput;
        private BoundsInt mapBounds;
        private float cameraHalfWidth;
        private float cameraHalfHeight;

        private void Awake()
        {
            cameraHalfHeight = cameraObject.orthographicSize;
            cameraHalfWidth = cameraHalfHeight * cameraObject.aspect;
        }

        private void Start()
        {
            mapBounds = levelGrid.CalculateBounds();
        }

        private void OnEnable()
        {
            inputManager.PlayerControls.Camera.Pan.performed += OnCameraPanPerformed;
            inputManager.PlayerControls.Camera.Pan.canceled += OnCameraPanCanceled;
        }

        private void OnDisable()
        {
            inputManager.PlayerControls.Camera.Pan.performed -= OnCameraPanPerformed;
            inputManager.PlayerControls.Camera.Pan.canceled -= OnCameraPanCanceled;
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
            if (panInput.HasValue)
            {
                var moveDirection = panSpeed * Time.deltaTime * new Vector3(panInput.Value.x, panInput.Value.y);
                var updatedPosition = cameraObject.transform.position - moveDirection;

                // Clamp the camera position to the defined bounds.
                updatedPosition.x = Mathf.Clamp(updatedPosition.x, mapBounds.min.x + cameraHalfWidth, mapBounds.max.x - cameraHalfWidth);
                updatedPosition.y = Mathf.Clamp(updatedPosition.y, mapBounds.min.y + cameraHalfHeight, mapBounds.max.y - cameraHalfHeight);

                cameraObject.transform.position = updatedPosition;
            }
        }
    }
}