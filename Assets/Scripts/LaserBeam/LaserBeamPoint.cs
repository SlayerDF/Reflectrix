using System;
using UnityEngine;

namespace Reflectrix.LaserBeam
{
    [Serializable]
    public struct LaserBeamPoint : ILaserBeamPoint
    {
        [SerializeField]
        private Vector3 origin;

        [SerializeField]
        private Vector3 direction;

        [SerializeField]
        private float intensity;

        [SerializeField]
        private Color color;

        public LaserBeamPoint(Vector3 origin, Vector3 direction, float intensity, Color color)
        {
            this.origin = origin;
            this.direction = direction;
            this.intensity = intensity;
            this.color = color;
        }

        public Vector3 Origin => origin;
        public Vector3 Direction => direction;
        public float Intensity => intensity;
        public Color Color => color;
    }
}
