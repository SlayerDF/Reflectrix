using System;
using Reflectrix.Utilities;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Traits
{
    [Serializable]
    public class OnDestroyFXTrait : IUniTrait
    {
        #region Serialized Fields

        [SerializeField]
        private PoolObject sfxPrefab;

        [SerializeField]
        private PoolObject vfxPrefab;

        #endregion

        private UniTraitContainer container;

        private UniTraitEventBus eventBus;

        #region IUniTrait Members

        public void OnEnable()
        {
            eventBus.Subscribe<DestructibleTrait.DestroyedEvent>(OnDestroyed);
        }

        public void OnDisable()
        {
            eventBus.Unsubscribe<DestructibleTrait.DestroyedEvent>(OnDestroyed);
        }

        public void InjectContainer(UniTraitContainer cont)
        {
            container = cont;
        }

        public void InjectEventBus(UniTraitEventBus bus)
        {
            eventBus = bus;
        }

        #endregion

        private void OnDestroyed(DestructibleTrait.DestroyedEvent @event)
        {
            if (sfxPrefab)
            {
                SceneObjectPool.Spawn<PoolObject>(sfxPrefab, out var sfxObj, true);
                sfxObj.gameObject.transform.position = container.transform.position;
                sfxObj.gameObject.SetActive(true);
            }

            if (vfxPrefab)
            {
                SceneObjectPool.Spawn<PoolObject>(vfxPrefab, out var vfxObj, true);
                vfxObj.gameObject.transform.position = container.transform.position;
                vfxObj.gameObject.SetActive(true);
            }
        }
    }
}
