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

        public LaserBeamPoint(Vector3 origin, Vector3 direction, float intensity)
        {
            this.origin = origin;
            this.direction = direction;
            this.intensity = intensity;
        }

        public Vector3 Origin => origin;
        public Vector3 Direction => direction;
        public float Intensity => intensity;
    }
}
