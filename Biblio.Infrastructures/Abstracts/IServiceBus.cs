using System;
using Biblio.Infrastructures.Concretes;

namespace Biblio.Infrastructures.Abstracts
{
    public interface IServiceBus
    {
        void Send<T>(T command) where T : CommandBase;
        void RegisterHandler<T>(Action<T> handler) where T : MessageBase;
    }
}
