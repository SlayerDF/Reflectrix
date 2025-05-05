using System;
using UnityEngine;

namespace Reflectrix.LaserBeam
{
    [Serializable]
    public struct LaserBeamPoint
    {
        [SerializeField]
        private Transform origin;

        [SerializeField]
        private Transform direction;

        [SerializeField]
        private float intensity;

        [SerializeField]
        private Color color;

# if UNITY_EDITOR
        public bool Initialized => origin != null && direction != null;
#endif

        public Vector3 Origin => origin.position;

        public Vector3 Direction => (direction.position - origin.position).normalized;

        public float Intensity => intensity;

        public Color Color => color;
    }
}