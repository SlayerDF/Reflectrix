using UnityEngine;

namespace Reflectrix
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls playerControls;

        public PlayerControls PlayerControls => playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            playerControls?.Dispose();
        }

        public void TestClick()
        {
            Debug.Log("TEst click!");
        }
    }
}