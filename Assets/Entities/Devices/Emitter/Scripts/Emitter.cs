using System.Linq;
using Reflectrix.LaserBeam;
using Reflectrix.Traits;
using UniTrait;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflectrix.Entities.Devices
{
    public class Emitter : UniTraitContainer, IBeamEmitter, IBeamReceiver
    {
        #region Serialized Fields

        [SerializeField]
        [AutoAddTrait]
        private LaserEmitterTrait laserEmitterTrait;

        [SerializeField]
        [AutoAddTrait]
        private LaserReceiverTrait laserReceiverTrait;

        [SerializeField]
        [AutoAddTrait]
        private DestructibleTrait destructibleTrait;

        [SerializeField]
        [AutoAddTrait]
        private OnDestroyFXTrait onDestroyFXTrait;

        #endregion

        #region Event Functions

        protected override void OnEnable()
        {
            base.OnEnable();

            EventBus.Subscribe<DestructibleTrait.DestroyedEvent>(OnEmitterDestroyed);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventBus.Unsubscribe<DestructibleTrait.DestroyedEvent>(OnEmitterDestroyed);
        }

        #endregion

        #region IBeamEmitter Members

        public ILaserBeamPoint[] Emit()
        {
            return laserEmitterTrait.Emit();
        }

        #endregion

        #region IBeamReceiver Members

        public bool Receive(ILaserBeamPoint[] beamPoints)
        {
            if (!laserReceiverTrait.Receive(beamPoints).Any())
            {
                return false;
            }

            destructibleTrait.Destroy(DestructibleTrait.DestructionReason.LaserBeam);

            return true;
        }

        #endregion

        private void OnEmitterDestroyed(DestructibleTrait.DestroyedEvent @event)
        {
            if (@event.Reason == DestructibleTrait.DestructionReason.LaserBeam)
            {
                SceneManager.LoadScene("Scenes/LoseScreen");
            }
        }
    }
}
