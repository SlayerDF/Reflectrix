using UnityEngine;

namespace Reflectrix.CameraController
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private MouseCameraInputHandler mouseCameraInputHandlerPrefab;

        [SerializeField]
        private TouchCameraInputHandler touchCameraInputHandlerPrefab;

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private Camera cameraObject;

        [SerializeField]
        private InputManager inputManager;

        private ICameraInputHandler cameraInputHandler;

        private GameObject cameraInputGameObject;

        private float cameraHalfWidth;
        private float cameraHalfHeight;
        private BoundsInt mapBounds;
        private float currentZoom;

        private void Awake()
        {
            currentZoom = cameraObject.orthographicSize;

#if UNITY_ANDROID && !UNITY_EDITOR
            var cameraInput = Instantiate(touchCameraInputHandlerPrefab);
            cameraInputGameObject = cameraInput.gameObject;
            cameraInputHandler = cameraInput;
#else
            var cameraInput = Instantiate(mouseCameraInputHandlerPrefab);
            cameraInputGameObject = cameraInput.gameObject;
            cameraInputHandler = cameraInput;
#endif
        }

        private void Start()
        {
            mapBounds = levelGrid.CalculateBounds();
            cameraInputHandler.Initialize(this, inputManager.PlayerControls);
        }

        private void OnDestroy()
        {
            if (cameraInputGameObject != null)
            {
                Destroy(cameraInputGameObject);
            }
        }

        public void ApplyZoom(float zoomValue, float zoomSpeed, float minZoom, float maxZoom)
        {
            var newSize = cameraObject.orthographicSize - zoomValue * zoomSpeed;
            cameraObject.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            currentZoom = cameraObject.orthographicSize;

            // Adjust the camera position to keep it within bounds.
            SetClampedCameraPosition(cameraObject.transform.position);
        }

        public void ApplyPan(Vector3 delta, float panSpeed)
        {
            var speed = panSpeed * currentZoom;
            var moveDirection = speed * Time.deltaTime * delta;
            var updatedPosition = cameraObject.transform.position - moveDirection;
            SetClampedCameraPosition(updatedPosition);
        }

        private void SetClampedCameraPosition(Vector3 cameraPosition)
        {
            cameraHalfHeight = cameraObject.orthographicSize;
            cameraHalfWidth = cameraHalfHeight * cameraObject.aspect;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, mapBounds.min.x + cameraHalfWidth, mapBounds.max.x - cameraHalfWidth);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, mapBounds.min.y + cameraHalfHeight, mapBounds.max.y - cameraHalfHeight);

            cameraObject.transform.position = cameraPosition;
        }
    }
}