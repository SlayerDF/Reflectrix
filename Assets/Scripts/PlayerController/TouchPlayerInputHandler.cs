using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Reflectrix.PlayerController
{
    internal class TouchPlayerInputHandler : MonoBehaviour, IPlayerInputHandler
    {
        private PlayerController playerController;
        private PlayerControls controls;
        private bool isInitialized = false;
        private bool processTouch = false;

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

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (processTouch)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    var pointerPosition = Touchscreen.current.position.ReadValue();
                    playerController.OnClick(pointerPosition);
                }

                processTouch = false;
            }
        }

        private void OnTouchPerformed(InputAction.CallbackContext context)
        {
            processTouch = true;
        }
    }
}