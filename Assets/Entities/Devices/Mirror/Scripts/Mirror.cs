using System.Collections.Generic;
using Reflectrix.Extensions;
using Reflectrix.LaserBeam;
using UnityEngine;

namespace Reflectrix.Entities.Devices
{
    public class Mirror : MonoBehaviour, IBeamReceiverAndEmitter
    {
        #region Serialized Fields

        [SerializeField]
        private Collider2D mirrorCollider;

        #endregion

        #region IBeamReceiverAndEmitter Members

        public ILaserBeamPoint[] ReceiveAndEmit(ILaserBeamPoint[] beamPoints)
        {
            var reflectedBeams = new List<ILaserBeamPoint>();

            foreach (var beam in beamPoints)
            {
                var hit = Physics2D.Raycast(beam.Origin, beam.Direction, 100f);

                if (hit.collider != mirrorCollider)
                {
                    continue;
                }

                const float dotThreshold = 0.99f;
                var hitTopEdge = Vector2.Dot(hit.normal, -hit.transform.up) > dotThreshold;
                var hitBottomEdge = Vector2.Dot(hit.normal, hit.transform.up) > dotThreshold;

                if (!hitTopEdge && !hitBottomEdge)
                {
                    continue;
                }

                var reflectedDir = Vector3.Reflect(beam.Direction, transform.up);

                reflectedBeams.Add(new LaserBeamPoint(hit.point.ToVector3(), reflectedDir, beam.Intensity));
            }

            return reflectedBeams.ToArray();
        }

        #endregion
    }
}
