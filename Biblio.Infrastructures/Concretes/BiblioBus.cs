using System;
using System.Collections.Generic;
using Biblio.Infrastructures.Abstracts;
using Biblio.Logging.Abstracts;

namespace Biblio.Infrastructures.Concretes
{
    public class BiblioServiceBus : IServiceBus, IDisposable, IEventBus
    {
        private readonly Dictionary<Type, List<Action<MessageBase>>> _routes = new Dictionary<Type, List<Action<MessageBase>>>();
        private readonly ILogService _logService;

        public BiblioServiceBus(ILogService logService)
        {
            this._logService = logService;
        }

        public void Send<T>(T command) where T : CommandBase
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            List<Action<MessageBase>> handlers;
            if (this._routes.TryGetValue(typeof (T), out handlers))
            {
                this._logService.LoggerTrace($"Comando {command.GetType()}; AggregateId {command.AggregateId}");
                handlers[0](command);
            }
            else
            {
                this._logService.LoggerTrace($"Nessun CommandHandler Trovato per il Comando {command.GetType()}");
                throw new Exception($"Nessun CommandHandler Trovato per il Comando {command.GetType()}");
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : MessageBase
        {
            List<Action<MessageBase>> handlers;
            if (!this._routes.TryGetValue(typeof (T), out handlers))
            {
                handlers = new List<Action<MessageBase>>();
                this._routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<MessageBase, T>(x => handler(x)));
        }

        public void Publish(EventBase @event)
        {
            List<Action<MessageBase>> handlers;
            if (!this._routes.TryGetValue(@event.GetType(), out handlers)) return;

            foreach (var handler in handlers)
            {
                this._logService.LoggerTrace($"Evento {@event.GetType()}; AggregateId {@event.AggregateId}");
                handler(@event);
            }
        }

        #region Dispose
        private bool _disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Qui si devono liberare le risorse eventualmente allocate da questo oggetto
                    // In questo caso ... nothing to do!!!
                }
            }
            this._disposed = true;
        }
        #endregion
    }
}
