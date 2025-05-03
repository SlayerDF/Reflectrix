using System;
using System.Collections.Generic;
using UniTrait.Interfaces;
using UnityEngine;

namespace UniTrait
{
    public class UniTraitContainer : MonoBehaviour
    {
        private readonly Dictionary<Type, IUniTrait> traits = new();
        private readonly List<IFixedUpdatableUniTrait> fixedUpdatables = new();
        private readonly List<ILateUpdatableUniTrait> lateUpdatables = new();
        private readonly List<IUpdatableUniTrait> updatables = new();
        private readonly UniTraitEventBus eventBus = new();

        #region Event Functions

        protected virtual void Update()
        {
            ManualUpdate();
        }

        protected virtual void FixedUpdate()
        {
            ManualFixedUpdate();
        }

        protected virtual void LateUpdate()
        {
            ManualLateUpdate();
        }

        #endregion

        public void ManualUpdate()
        {
            foreach (var trait in updatables)
            {
                trait.OnUpdate();
            }
        }

        public void ManualFixedUpdate()
        {
            foreach (var trait in fixedUpdatables)
            {
                trait.OnFixedUpdate();
            }
        }

        public void ManualLateUpdate()
        {
            foreach (var trait in lateUpdatables)
            {
                trait.OnLateUpdate();
            }
        }

        public void AddTrait<T>(T trait) where T : IUniTrait
        {
            traits[typeof(T)] = trait;

            if (trait is IUpdatableUniTrait updatable)
            {
                updatables.Add(updatable);
            }

            if (trait is IFixedUpdatableUniTrait fixedUpdatable)
            {
                fixedUpdatables.Add(fixedUpdatable);
            }

            if (trait is ILateUpdatableUniTrait lateUpdatable)
            {
                lateUpdatables.Add(lateUpdatable);
            }

            if (trait is IEventListenerUniTrait eventListener)
            {
                eventListener.InjectEventBus(eventBus);
            }
        }

        public T GetTrait<T>() where T : IUniTrait
        {
            return traits.TryGetValue(typeof(T), out var trait) ? (T)trait : default;
        }
    }
}
