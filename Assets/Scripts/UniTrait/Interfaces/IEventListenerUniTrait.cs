namespace UniTrait.Interfaces
{
    public interface IEventListenerUniTrait : IUniTrait
    {
        void InjectEventBus(UniTraitEventBus eventBus);
    }
}
