using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Reflectrix
{
    public class SplashScreen : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private string nextScene;

        #endregion

        #region Event Functions

        private void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                LoadNextScene();
            }
#else
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                LoadNextScene();
            }

            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                LoadNextScene();
            }
#endif
        }

        #endregion

        private void LoadNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
