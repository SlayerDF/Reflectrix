using System.Linq;
using Reflectrix.LaserBeam;
using Reflectrix.Traits;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Entities.Devices
{
    public class Receiver : UniTraitContainer, IBeamReceiver
    {
        #region Serialized Fields

        [SerializeField]
        private float targetIntensity;

        [Header("Traits")]
        [SerializeField]
        [AutoAddTrait]
        private LaserReceiverTrait laserReceiverTrait;

        #endregion

        public float CurrentIntensity { get; private set; }

        #region IBeamReceiver Members

        public bool Receive(ILaserBeamPoint[] beamPoint)
        {
            UpdateIntensity(laserReceiverTrait.Receive(beamPoint).Sum(x => x.Intensity));

            return CurrentIntensity >= targetIntensity;
        }

        #endregion

        public void UpdateIntensity(float value)
        {
            if ((CurrentIntensity = value) >= targetIntensity)
            {
                // TODO: display game win screen
            }
        }
    }
}
