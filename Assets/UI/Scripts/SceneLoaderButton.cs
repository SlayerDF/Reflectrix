using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Reflectrix.UI
{
    public class SceneLoaderButton : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private Button button;

        [SerializeField]
        private string sceneName;

        #endregion

        #region Event Functions

        private void OnEnable()
        {
            button.onClick.AddListener(LoadScene);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(LoadScene);
        }

        #endregion

        private void LoadScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
