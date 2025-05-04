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

        private void OnEmitterDestroyed(DestructibleTrait.DestroyedEvent @event)
        {
            if (@event.Reason == DestructibleTrait.DestructionReason.LaserBeam)
            {
                // TODO: display game over screen
            }
        }
    }
}
