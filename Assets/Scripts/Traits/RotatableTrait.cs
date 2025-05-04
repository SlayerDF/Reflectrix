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

        #region IUniTrait Members

        public void InjectContainer(UniTraitContainer cont)
        {
            container = cont;
        }

        #endregion

        [UniTraitDebugAction("Rotate Left")]
        public void RotateLeft()
        {
            var transform = container.transform;
            transform.Rotate(Vector3.forward, rotationAngleStep);
        }

        [UniTraitDebugAction("Rotate Right")]
        public void RotateRight()
        {
            var transform = container.transform;
            transform.Rotate(Vector3.forward, -rotationAngleStep);
        }
    }
}
