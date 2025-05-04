using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniTrait.Tests
{
    public class UniTraitContainerTests
    {
        private UniTraitContainer container;
        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject("TestObject");
            container = gameObject.AddComponent<UniTraitContainer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void AddTrait_RegistersAndResolvesTrait()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            var resolved = container.GetTrait<DummyTrait>();
            Assert.AreSame(trait, resolved);
        }

        [Test]
        public void AddTrait_ThrowsException_WhenDuplicateTraitAdded()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            var ex = Assert.Throws<ArgumentException>(() => container.AddTrait(trait));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public void ManualAwake_CallsOnAwake()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualAwake();

            Assert.IsTrue(trait.Awoke);
        }

        [Test]
        public void ManualStart_CallsOnStart()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualStart();

            Assert.IsTrue(trait.Started);
        }

        [Test]
        public void ManualUpdate_CallsOnUpdate()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualUpdate();

            Assert.IsTrue(trait.Updated);
        }

        [Test]
        public void ManualFixedUpdate_CallsOnFixedUpdate()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualFixedUpdate();

            Assert.IsTrue(trait.FixedUpdated);
        }

        [Test]
        public void ManualLateUpdate_CallsOnLateUpdate()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualLateUpdate();

            Assert.IsTrue(trait.LateUpdated);
        }

        [Test]
        public void ManualEnable_CallsOnEnable()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualEnable();

            Assert.IsTrue(trait.Enabled);
        }

        [Test]
        public void ManualDisable_CallsOnDisable()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualDisable();

            Assert.IsTrue(trait.Disabled);
        }

        [Test]
        public void ManualDestroy_CallsOnDestroy()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualDestroy();

            Assert.IsTrue(trait.Destroyed);
        }

        [Test]
        public void ManualValidate_CallsOnValidate()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualValidate();

            Assert.IsTrue(trait.Validated);
        }

        [Test]
        public void ManualDrawGizmos_CallsOnDrawGizmos()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualDrawGizmos();

            Assert.IsTrue(trait.DrewGizmos);
        }

        [Test]
        public void ManualDrawGizmos_CallsOnDrawGizmosSelected()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            container.ManualDrawGizmosSelected();

            Assert.IsTrue(trait.DrewGizmosSelected);
        }

        [Test]
        public void AddTrait_InjectsContainer()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            Assert.IsNotNull(trait.Container);
        }

        [Test]
        public void AddTrait_InjectsEventBus()
        {
            var trait = new DummyTrait();
            container.AddTrait(trait);

            Assert.IsNotNull(trait.EventBus);
        }

        private class DummyTrait : IUniTrait
        {
            public bool Awoke { get; private set; }
            public bool Started { get; private set; }
            public bool Enabled { get; private set; }
            public bool Disabled { get; private set; }
            public bool Updated { get; private set; }
            public bool FixedUpdated { get; private set; }
            public bool LateUpdated { get; private set; }
            public bool Destroyed { get; private set; }
            public bool Validated { get; private set; }
            public bool DrewGizmos { get; private set; }
            public bool DrewGizmosSelected { get; private set; }
            public UniTraitContainer Container { get; private set; }
            public UniTraitEventBus EventBus { get; private set; }

            #region IUniTrait Members

            public void OnAwake()
            {
                Awoke = true;
            }

            public void OnStart()
            {
                Started = true;
            }

            public void OnEnable()
            {
                Enabled = true;
            }

            public void OnDisable()
            {
                Disabled = true;
            }

            public void OnUpdate()
            {
                Updated = true;
            }

            public void OnFixedUpdate()
            {
                FixedUpdated = true;
            }

            public void OnLateUpdate()
            {
                LateUpdated = true;
            }

            public void OnDestroy()
            {
                Destroyed = true;
            }

            public void OnValidate()
            {
                Validated = true;
            }

            public void OnDrawGizmos()
            {
                DrewGizmos = true;
            }

            public void OnDrawGizmosSelected()
            {
                DrewGizmosSelected = true;
            }

            public void InjectContainer(UniTraitContainer container)
            {
                Container = container;
            }

            public void InjectEventBus(UniTraitEventBus eventBus)
            {
                EventBus = eventBus;
            }

            #endregion
        }
    }
}
