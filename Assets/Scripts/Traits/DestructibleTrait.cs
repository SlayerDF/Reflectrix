using System;
using UniTrait;
using Object = UnityEngine.Object;

namespace Reflectrix.Traits
{
    [Serializable]
    public class DestructibleTrait : IUniTrait
    {
        #region DestructionReason enum

        public enum DestructionReason
        {
            LaserBeam,
            UserAction
        }

        #endregion

        private UniTraitContainer container;
        private UniTraitEventBus eventBus;

        public bool IsDestroyed { get; private set; }

        #region IUniTrait Members

        public void InjectContainer(UniTraitContainer cont)
        {
            container = cont;
        }

        public void InjectEventBus(UniTraitEventBus bus)
        {
            eventBus = bus;
        }

        #endregion

        public void Destroy(DestructionReason reason)
        {
            IsDestroyed = true;

            eventBus.Publish(new DestroyedEvent { Reason = reason });

            Object.Destroy(container.gameObject);
        }

        #region Nested type: ${0}

        public struct DestroyedEvent
        {
            public DestructionReason Reason;
        }

        #endregion
    }
}
