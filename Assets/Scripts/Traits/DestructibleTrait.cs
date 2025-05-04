using System;
using UniTrait;
using Object = UnityEngine.Object;

namespace Reflectrix.Traits
{
    [Serializable]
    public class DestructibleTrait : IUniTrait
    {
        #region Delegates

        public delegate void DestructionHandler(UniTraitContainer container, DestructionReason reason);

        #endregion

        #region DestructionReason enum

        public enum DestructionReason
        {
            LaserBeam,
            UserAction
        }

        #endregion

        private UniTraitContainer container;

        public bool IsDestroyed { get; private set; }

        #region IUniTrait Members

        public void InjectContainer(UniTraitContainer cont)
        {
            container = cont;
        }

        #endregion

        public event DestructionHandler OnDestroyed;

        public void Destroy(DestructionReason reason)
        {
            IsDestroyed = true;

            OnDestroyed?.Invoke(container, reason);

            Object.Destroy(container.gameObject);
        }
    }
}
