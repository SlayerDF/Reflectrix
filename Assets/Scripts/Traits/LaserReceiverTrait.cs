using System;
using System.Linq;
using Reflectrix.LaserBeam;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Traits
{
    [Serializable]
    public class LaserReceiverTrait : IUniTrait
    {
        #region Serialized Fields

        [SerializeField]
        private Transform[] receivers;

        #endregion

        private UniTraitEventBus eventBus;

        #region IUniTrait Members

        public void InjectEventBus(UniTraitEventBus bus)
        {
            eventBus = bus;
        }

        #endregion

        public ILaserBeamPoint[] Receive(ILaserBeamPoint[] beamPoints)
        {
            var receivedBeamPoints = beamPoints
                .Where(beam => receivers.Any(r => IsPointOnRay(beam.Origin, beam.Direction, r.position)))
                .ToArray();

            eventBus.Publish(new ReceivedEvent { BeamPoints = receivedBeamPoints });

            return beamPoints;
        }

        private bool IsPointOnRay(Vector3 origin, Vector3 direction, Vector3 point)
        {
            var toPoint = point - origin;
            return Vector3.Cross(toPoint, direction).sqrMagnitude == 0f
                   && Vector3.Dot(toPoint, direction) >= 0f;
        }

        #region Nested type: ${0}

        public struct ReceivedEvent
        {
            public ILaserBeamPoint[] BeamPoints;
        }

        #endregion
    }
}
