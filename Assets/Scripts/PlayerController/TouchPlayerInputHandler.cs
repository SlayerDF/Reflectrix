using UnityEngine;
using UnityEngine.InputSystem;

namespace Reflectrix.PlayerController
{
    internal class TouchPlayerInputHandler : MonoBehaviour, IPlayerInputHandler
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

            controls.Player.Touch.performed += OnTouchPerformed;
            isInitialized = true;
        }

        private void OnDestroy()
        {
            if (controls != null)
            {
                controls.Player.MouseLeftClick.performed -= OnTouchPerformed;
            }

            isInitialized = false;
        }

        private void OnTouchPerformed(InputAction.CallbackContext context)
        {
        }
    }
}