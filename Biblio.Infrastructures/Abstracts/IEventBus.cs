using Biblio.Infrastructures.Concretes;

namespace Biblio.Infrastructures.Abstracts
{
    public interface IEventBus
    {
        void Publish(EventBase @event);
    }
}
