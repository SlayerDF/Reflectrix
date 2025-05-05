using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.PlayerController
{
    public class MousePlayerInputHandler : MonoBehaviour, IPlayerInputHandler
    {
        private PlayerController playerController;
        private PlayerControls controls;
        private bool isInitialized = false;

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

        private void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            var pointerPosition = Mouse.current.position.ReadValue();
            playerController.OnClick(pointerPosition);
        }
    }
}