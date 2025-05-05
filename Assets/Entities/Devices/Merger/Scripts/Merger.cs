using System.Linq;
using Reflectrix.LaserBeam;
using Reflectrix.Traits;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Entities.Devices.Merger.Scripts
{
    public class Merger : UniTraitContainer, IBeamReceiverAndEmitter
    {
        #region Serialized Fields

        [SerializeField]
        [AutoAddTrait]
        private LaserReceiverTrait laserReceiverTrait;

        [SerializeField]
        [AutoAddTrait]
        private LaserEmitterTrait laserEmitterTrait;

        [SerializeField]
        [AutoAddTrait]
        private RotatableTrait rotatableTrait;

        #endregion

        #region IBeamReceiverAndEmitter Members

        public ILaserBeamPoint[] ReceiveAndEmit(ILaserBeamPoint[] beamPoints)
        {
            var receivedBeamPoints = laserReceiverTrait.Receive(beamPoints);
            var incomingIntensity = receivedBeamPoints.Sum(x => x.Intensity);

            if (receivedBeamPoints.Length == 0 || incomingIntensity <= 0)
            {
                return new ILaserBeamPoint[] { };
            }

            var emittedBeamPoints = laserEmitterTrait.Emit();
            var splitIntensity = incomingIntensity / emittedBeamPoints.Length;

            return emittedBeamPoints
                .Select<ILaserBeamPoint, ILaserBeamPoint>(x =>
                    new LaserBeamPoint(x.Origin, x.Direction, splitIntensity, x.Color)).ToArray();
        }

        #endregion
    }
}
