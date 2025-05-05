using UnityEngine;

namespace Reflectrix.LaserBeam
{
    public interface ILaserBeamPoint
    {
        Vector3 Direction { get; }
        Vector3 Origin { get; }
        float Intensity { get; }
        Color Color { get; }
    }
}
