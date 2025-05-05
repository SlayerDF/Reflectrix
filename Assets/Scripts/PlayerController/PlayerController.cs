using UnityEngine;

namespace Reflectrix.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private MousePlayerInputHandler mousePlayerInputHandlerPrefab;

        [SerializeField]
        private TouchPlayerInputHandler touchPlayerInputHandlerPrefab;

        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private LevelBuilder levelBuilder;

        [SerializeField]
        private Camera mainCamera;

        private GameObject playerInputGameObject;

        private IPlayerInputHandler playerInputHandler;

        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var playerInput = Instantiate(touchPlayerInputHandlerPrefab);
            playerInputGameObject = playerInput.gameObject;
            playerInputHandler = playerInput;
#else
            var playerInput = Instantiate(mousePlayerInputHandlerPrefab);
            playerInputGameObject = playerInput.gameObject;
            playerInputHandler = playerInput;
#endif
        }

        private void Start()
        {
            playerInputHandler.Initialize(this, inputManager.PlayerControls);
        }

        public void OnClick(Vector2 screenPosition)
        {
            var worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y));
            var tile = levelGrid.Grid.WorldToCell(worldPos);
            if (!levelGrid.FloorTileMap.GetTile(tile))
            {
                return;
            }

            if (!levelBuilder.IsCellFree(tile))
            {
                Debug.Log("Current tile is occupied.");
                return;
            }

            levelBuilder.UpdateCellState(tile, false);
            Debug.Log($"UpdateCellState {tile.x}, {tile.y}");
        }
    }
}