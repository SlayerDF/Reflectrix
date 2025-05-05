using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Reflectrix.PlayerController
{
    internal class TouchPlayerInputHandler : MonoBehaviour, IPlayerInputHandler
    {
        private const float tapMaxDuration = 0.2f;
        private const float TapMaxMovement = 30; // pixels
        private PlayerController playerController;
        private PlayerControls controls;
        private bool isInitialized = false;
        private bool processTouch = false;
        private Vector2 touchStartPosition;
        private bool isTouching;

        public void Initialize(PlayerController playerController, PlayerControls controls)
        {
            if (isInitialized)
            {
                return;
            }

            this.playerController = playerController;
            this.controls = controls;

            controls.Player.Touch.started += OnTouchStarted;
            controls.Player.Touch.canceled += OnTouchCanceled;
            isInitialized = true;
        }

        private void OnTouchCanceled(InputAction.CallbackContext context)
        {
            if (!isTouching)
                return;

            isTouching = false;
            var endPos = Touchscreen.current.position.ReadValue();
            var movement = Vector2.Distance(endPos, touchStartPosition);

            if (movement <= TapMaxMovement)
            {
                processTouch = true;
            }
        }

        private void OnTouchStarted(InputAction.CallbackContext context)
        {
            isTouching = true;
            touchStartPosition = Touchscreen.current.position.ReadValue();
        }

        private void OnDestroy()
        {
            if (controls != null)
            {
                controls.Player.Touch.started -= OnTouchStarted;
                controls.Player.Touch.canceled -= OnTouchCanceled;
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
                    playerController.OnClick(touchStartPosition);
                }

                processTouch = false;
            }
        }
    }
}