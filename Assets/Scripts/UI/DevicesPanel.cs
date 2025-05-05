using Reflectrix.Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Reflectrix
{
    public class DevicesPanel : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private GameObject uiBlocker;

        [SerializeField]
        private Button btn_Mirror;

        [SerializeField]
        private Button btn_Splitter;

        [SerializeField]
        private Button btn_Merger;

        // TODO: Use another type of enum or move it.
        public event EventHandler<TileObjectType> OnDeviceSelected;

        private void OnEnable()
        {
            btn_Mirror.onClick.AddListener(OnMirrorButtonClicked);
            btn_Splitter.onClick.AddListener(OnSplitterButtonClicked);
            btn_Merger.onClick.AddListener(OnMergerButtonClicked);
        }

        private void OnMergerButtonClicked()
        {
            OnDeviceSelected?.Invoke(this, TileObjectType.Merger);
        }

        private void OnSplitterButtonClicked()
        {
            OnDeviceSelected?.Invoke(this, TileObjectType.Splitter);
        }

        private void OnMirrorButtonClicked()
        {
            OnDeviceSelected?.Invoke(this, TileObjectType.Mirror);
        }

        private void OnDisable()
        {
            btn_Mirror.onClick.RemoveListener(OnMirrorButtonClicked);
            btn_Splitter.onClick.RemoveListener(OnSplitterButtonClicked);
            btn_Merger.onClick.RemoveListener(OnMergerButtonClicked);
        }

        public void ShowDevicesPanel(Vector2 screenPoint)
        {
            rectTransform.position = screenPoint;
            gameObject.SetActive(true);
            uiBlocker.SetActive(true);
        }

        public void HideDevicesPanel()
        {
            gameObject.SetActive(false);
            uiBlocker.SetActive(false);
        }
    }
}