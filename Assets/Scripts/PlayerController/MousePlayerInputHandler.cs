using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Reflectrix.PlayerController
{
    public class MousePlayerInputHandler : MonoBehaviour, IPlayerInputHandler
    {
        private PlayerController playerController;
        private PlayerControls controls;
        private bool isInitialized = false;
        private bool processMouseClick = false;

        public void Initialize(PlayerController playerController, PlayerControls controls)
        {
            if (isInitialized)
            {
                return;
            }

            this.playerController = playerController;
            this.controls = controls;

            controls.Player.MouseLeftClick.performed += OnMouseLeftClick;
            isInitialized = true;
        }

        private void OnDestroy()
        {
            if (controls != null)
            {
                controls.Player.MouseLeftClick.performed -= OnMouseLeftClick;
            }

            isInitialized = false;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (processMouseClick)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    var pointerPosition = Mouse.current.position.ReadValue();
                    playerController.OnClick(pointerPosition);
                }

                processMouseClick = false;
            }
        }

        private void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            processMouseClick = true;
        }
    }
}