using Reflectrix.Traits;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Reflectrix
{
    public class DeviceEditingPanel : MonoBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Button uiBlocker;

        [SerializeField]
        private Button btn_RotateLeft;

        [SerializeField]
        private Button btn_RotateRight;

        private RotatableTrait currentRotatableTrait;

        private void OnEnable()
        {
            btn_RotateLeft.onClick.AddListener(OnRotateLeftButtonClicked);
            btn_RotateRight.onClick.AddListener(OnRotateRightButtonClicked);
            uiBlocker.onClick.AddListener(HidePanel);
        }

        public void ShowPanel(Vector2 screenPoint, RotatableTrait rotatableTrait)
        {
            currentRotatableTrait = rotatableTrait;
            rectTransform.position = screenPoint;
            gameObject.SetActive(true);
            uiBlocker.gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            currentRotatableTrait = null;
            gameObject.SetActive(false);
            uiBlocker.gameObject.SetActive(false);
        }

        private void OnRotateRightButtonClicked()
        {
            if (currentRotatableTrait == null)
            {
                return;
            }

            currentRotatableTrait.RotateRight();
        }

        private void OnRotateLeftButtonClicked()
        {
            if (currentRotatableTrait == null)
            {
                return;
            }

            currentRotatableTrait.RotateLeft();
        }

        private void OnDisable()
        {
            btn_RotateLeft.onClick.RemoveListener(OnRotateLeftButtonClicked);
            btn_RotateRight.onClick.RemoveListener(OnRotateRightButtonClicked);
            uiBlocker.onClick.RemoveListener(HidePanel);
        }
    }
}