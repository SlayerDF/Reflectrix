using UnityEngine;

namespace Reflectrix.CameraController
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PanZoomSettings")]
    public class PanZoomSettigns : ScriptableObject
    {
        [Header("Pan Settings")]
        [SerializeField]
        private float panSpeed = 1f;

        [Header("Zoom Settings")]
        [SerializeField]
        private float zoomSpeed = 0.1f;

        [SerializeField]
        private float minZoom = 1f;

        [SerializeField]
        private float maxZoom = 5;

        public float PanSpeed => panSpeed;

        public float ZoomSpeed => zoomSpeed;

        public float MinZoom => minZoom;

        public float MaxZoom => maxZoom;
    }
}