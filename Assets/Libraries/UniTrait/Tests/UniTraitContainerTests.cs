using NUnit.Framework;
using UniTrait.Interfaces;
using UnityEngine;

namespace UniTrait.Tests
{
    public class UniTraitContainerTests
    {
        private UniTraitContainer container;
        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject("TestObj");
            container = gameObject.AddComponent<UniTraitContainer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void AddTrait_RegistersUpdatableTrait()
        {
            var trait = new DummyUpdatableTrait();
            container.AddTrait(trait);

            var resolved = container.GetTrait<DummyUpdatableTrait>();
            Assert.AreEqual(trait, resolved);

            container.ManualUpdate();
            Assert.IsTrue(trait.MethodCalled);
        }

        [Test]
        public void AddTrait_RegistersFixedUpdatableTrait()
        {
            var trait = new DummyFixedUpdatableTrait();
            container.AddTrait(trait);

            var resolved = container.GetTrait<DummyFixedUpdatableTrait>();
            Assert.AreEqual(trait, resolved);

            container.ManualFixedUpdate();
            Assert.IsTrue(trait.MethodCalled);
        }

        [Test]
        public void AddTrait_RegistersLateUpdatableTrait()
        {
            var trait = new DummyLateUpdatableTrait();
            container.AddTrait(trait);

            var resolved = container.GetTrait<DummyLateUpdatableTrait>();
            Assert.AreEqual(trait, resolved);

            container.ManualLateUpdate();
            Assert.IsTrue(trait.MethodCalled);
        }

        [Test]
        public void AddTrait_AssignsEventBusIfListener()
        {
            var trait = new DummyEventListenerTrait();
            container.AddTrait(trait);

            var resolved = container.GetTrait<DummyEventListenerTrait>();
            Assert.AreEqual(trait, resolved);
            Assert.IsNotNull(trait.EventBus);
        }

        private class DummyUpdatableTrait : IUpdatableUniTrait
        {
            public bool MethodCalled { get; private set; }

            public void OnUpdate()
            {
                MethodCalled = true;
            }
        }

        private class DummyFixedUpdatableTrait : IFixedUpdatableUniTrait
        {
            public bool MethodCalled { get; private set; }

            public void OnFixedUpdate()
            {
                MethodCalled = true;
            }
        }

        private class DummyLateUpdatableTrait : ILateUpdatableUniTrait
        {
            public bool MethodCalled { get; private set; }

            public void OnLateUpdate()
            {
                MethodCalled = true;
            }
        }

        private class DummyEventListenerTrait : IEventListenerUniTrait
        {
            public UniTraitEventBus EventBus { get; private set; }

            public void InjectEventBus(UniTraitEventBus eventBus)
            {
                EventBus = eventBus;
            }
        }
    }
}
