using Reflectrix.Traits;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Entities.Devices
{
    public class Emitter : UniTraitContainer
    {
        #region Serialized Fields

        [SerializeField]
        [AutoAddTrait]
        private LaserEmitterTrait laserEmitterTrait;

        [SerializeField]
        [AutoAddTrait]
        private RotatableTrait rotatableTrait;

        #endregion
    }
}
