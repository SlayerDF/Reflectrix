using Reflectrix.Assets.Scripts;
using Reflectrix.Traits;
using UniTrait;
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

        [SerializeField]
        private DevicesPanel devicesPanel;

        [SerializeField]
        private DeviceEditingPanel deviceEditingPanel;

        private GameObject playerInputGameObject;

        private IPlayerInputHandler playerInputHandler;

        private Vector3Int currentSelectedTile;

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

            devicesPanel.OnDeviceSelected += OnDeviceSelected;
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

            if (!levelBuilder.TryGetTile(tile, out var tileObject))
            {
                Debug.Log($"Tile is not available for editing.");
                return;
            }

            if (tileObject.Value.IsOccupied)
            {
                ProcessOccupiedTile(tileObject.Value, screenPosition);
            }
            else
            {
                // Show the devices panel at the clicked position to add new device.
                currentSelectedTile = tile;
                devicesPanel.ShowDevicesPanel(screenPosition);
            }
        }

        private void ProcessOccupiedTile(TileState tileObject, Vector2 screenPosition)
        {
            if (tileObject.TileObjectType == TileObjectType.None ||
                tileObject.TileObjectType == TileObjectType.Obstacle ||
                tileObject.TileObjectType == TileObjectType.PredifinedObject)
            {
                Debug.Log($"Tile is not available for editing.");
                return;
            }

            if (!tileObject.GameObject.TryGetTrait<RotatableTrait>(out var rotatableTrait))
            {
                Debug.LogError($"Device {tileObject.TileObjectType} does not have a RotatableTrait component.");
                return;
            }

            // Show the panel to edit devices rotation.
            deviceEditingPanel.ShowPanel(screenPosition, rotatableTrait);
        }

        private void OnDeviceSelected(object sender, TileObjectType tileObjectType)
        {
            levelBuilder.OccupyCell(currentSelectedTile, tileObjectType);
            devicesPanel.HideDevicesPanel();
        }
    }
}