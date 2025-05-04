using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UniTrait
{
    public class UniTraitContainer : MonoBehaviour
    {
        private readonly Dictionary<Type, int> traitsIndexes = new();
        private readonly List<IUniTrait> traitsList = new();
        protected readonly UniTraitEventBus EventBus = new();

        #region Event Functions

        protected virtual void Awake()
        {
            if (traitsList.Count == 0)
            {
                AutoAddTraits();
            }

            ManualAwake();
        }

        protected virtual void Start()
        {
            ManualStart();
        }

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

        protected virtual void OnEnable()
        {
            ManualEnable();
        }

        protected virtual void OnDisable()
        {
            ManualDisable();
        }

        #endregion

        public void ManualAwake()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnAwake();
            }
        }

        public void ManualStart()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnStart();
            }
        }

        public void ManualEnable()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnEnable();
            }
        }

        public void ManualDisable()
        {
            for (var i = traitsList.Count - 1; i >= 0; i--)
            {
                traitsList[i].OnDisable();
            }
        }

        public void ManualUpdate()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnUpdate();
            }
        }

        public void ManualFixedUpdate()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnFixedUpdate();
            }
        }

        public void ManualLateUpdate()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnLateUpdate();
            }
        }

        public void ManualDestroy()
        {
            for (var i = traitsList.Count - 1; i >= 0; i--)
            {
                traitsList[i].OnDestroy();
            }
        }

        public void ManualValidate()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnValidate();
            }
        }

        public void ManualDrawGizmos()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnDrawGizmos();
            }
        }

        public void ManualDrawGizmosSelected()
        {
            for (var i = 0; i < traitsList.Count; i++)
            {
                traitsList[i].OnDrawGizmosSelected();
            }
        }

        public void AddTrait<T>(T trait) where T : IUniTrait
        {
            var type = trait.GetType();

            if (traitsIndexes.ContainsKey(type))
            {
                throw new ArgumentException($"Trait of type {type} already exists");
            }

            traitsList.Add(trait);
            traitsIndexes[type] = traitsList.Count - 1;

            trait.InjectContainer(this);
            trait.InjectEventBus(EventBus);
        }

        public void AddTraits<T>(params T[] traits) where T : IUniTrait
        {
            foreach (var trait in traits)
            {
                AddTrait(trait);
            }
        }

        public T GetTrait<T>() where T : IUniTrait
        {
            return traitsIndexes.TryGetValue(typeof(T), out var traitIndex) ? (T)traitsList[traitIndex] : default;
        }

        public IEnumerable<IUniTrait> GetTraits()
        {
            return traitsList;
        }

        private void AutoAddTraits()
        {
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (var i = 0; i < fields.Length; i++)
            {
                if (fields[i].GetCustomAttribute<AutoAddTraitAttribute>() == null)
                {
                    continue;
                }

                if (!typeof(IUniTrait).IsAssignableFrom(fields[i].FieldType))
                {
                    continue;
                }

                if (fields[i].GetValue(this) is IUniTrait trait)
                {
                    AddTrait(trait);
                }
            }
        }

# if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            ManualDrawGizmos();
        }

        protected void OnDrawGizmosSelected()
        {
            ManualDrawGizmosSelected();
        }

        protected void OnValidate()
        {
            traitsList.Clear();
            traitsIndexes.Clear();
            AutoAddTraits();
            ManualValidate();
        }
#endif
    }
}
