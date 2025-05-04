using System;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Traits
{
    [Serializable]
    public class RotatableTrait : IUniTrait
    {
        #region Serialized Fields

        [SerializeField]
        private float rotationAngleStep;

        #endregion

        private UniTraitContainer container;
        private UniTraitEventBus eventBus;

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


        [UniTraitDebugAction("Rotate Left")]
        public void RotateLeft()
        {
            var transform = container.transform;
            transform.Rotate(Vector3.forward, rotationAngleStep);

            eventBus.Publish(new RotatedEvent { ForwardVector = transform.forward, DeltaAngle = rotationAngleStep });
        }

        [UniTraitDebugAction("Rotate Right")]
        public void RotateRight()
        {
            var transform = container.transform;
            transform.Rotate(Vector3.forward, -rotationAngleStep);

            eventBus.Publish(new RotatedEvent { ForwardVector = transform.forward, DeltaAngle = -rotationAngleStep });
        }

        #region Nested type: ${0}

        public struct RotatedEvent
        {
            public Vector3 ForwardVector;
            public float DeltaAngle;
        }

        #endregion
    }
}
