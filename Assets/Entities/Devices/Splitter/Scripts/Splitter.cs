using System.Linq;
using Reflectrix.LaserBeam;
using Reflectrix.Traits;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Entities.Devices
{
    public class Splitter : UniTraitContainer, IBeamReceiverAndEmitter
    {
        #region Serialized Fields

        [SerializeField]
        private LaserReceiverTrait laserReceiverTrait;

        [SerializeField]
        private LaserEmitterTrait laserEmitterTrait;

        #endregion

        #region IBeamReceiverAndEmitter Members

        public ILaserBeamPoint[] ReceiveAndEmit(ILaserBeamPoint[] beamPoints)
        {
            var receivedBeamPoints = laserReceiverTrait.Receive(beamPoints);

            if (receivedBeamPoints.Length == 0)
            {
                return new ILaserBeamPoint[] { };
            }

            var incomingIntensity = receivedBeamPoints.Sum(x => x.Intensity);
            var emittedBeamPoints = laserEmitterTrait.Emit();
            var splitIntensity = incomingIntensity / emittedBeamPoints.Length;

            return emittedBeamPoints
                .Select<ILaserBeamPoint, ILaserBeamPoint>(x =>
                    new LaserBeamPoint(x.Origin, x.Direction, splitIntensity, x.Color)).ToArray();
        }

        #endregion
    }
}
