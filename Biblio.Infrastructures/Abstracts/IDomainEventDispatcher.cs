using Biblio.Infrastructures.Concretes;

namespace Biblio.Infrastructures.Abstracts
{
    public interface IDomainEventDispatcher
    {
        void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : EventBase;
    }
}