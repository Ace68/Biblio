namespace Biblio.Infrastructures.Abstracts
{
    public interface IEventHandler<in T>
    {
        void Handle(T @event);
    }
}
