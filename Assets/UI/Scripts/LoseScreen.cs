using Reflectrix.RemoteConfig;
using TMPro;
using UnityEngine;

namespace Reflectrix.UI
{
    public class LoseScreen : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private TMP_Text title;

        #endregion

        #region Event Functions

        private void Awake()
        {
            title.text = RemoteConfigFetcher.Config.loseTitle;
        }

        #endregion
    }
}
