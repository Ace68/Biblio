using System;
using CommonDomain.Core;
using CommonDomain.Persistence;

namespace Biblio.Domain.CommandsHandlers
{
    public abstract class CommandsHandler
    {
        protected readonly IRepository BiblioRepository;

        protected CommandsHandler(IRepository biblioRepository)
        {
            if (biblioRepository == null)
                throw new ArgumentNullException(nameof(biblioRepository));

            this.BiblioRepository = biblioRepository;
        }

        public TEntity Get<TEntity>(Guid id, IRepository repository) where TEntity : AggregateBase
        {
            var aggregate = repository.GetById<TEntity>(id);

            if (aggregate == null)
                throw new Exception($"{typeof(TEntity).Name} Not Found!");

            return aggregate;
        }
    }
}
