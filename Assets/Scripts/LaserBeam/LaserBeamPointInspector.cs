using System;
using UnityEngine;

namespace Reflectrix.LaserBeam
{
    [Serializable]
    public struct LaserBeamPointInspector : ILaserBeamPoint
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

        public LaserBeamPoint ToLaserBeamPoint()
        {
            return new LaserBeamPoint(origin.position, direction.position, intensity, color);
        }

        public Vector3 Origin => origin.position;
        public Vector3 Direction => (direction.position - origin.position).normalized;
        public float Intensity => intensity;
        public Color Color => color;
    }
}
