namespace Assets.Scripts.Systems.Observables
{
    public interface IEventListener<T>
    {
        void Notify(T e);
    }
}
