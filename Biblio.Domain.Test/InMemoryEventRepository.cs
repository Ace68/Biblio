using System;
using System.Collections.Generic;
using System.Linq;
using CommonDomain;
using CommonDomain.Persistence;

using Biblio.EventStore.Persistence.Repositories;
using Biblio.Infrastructures.Concretes;

namespace Biblio.Domain.Test
{ /// <summary>
  /// https://github.com/luizdamim/NEventStoreExample/tree/master/NEventStoreExample.Test
  /// </summary>
    public class InMemoryEventRepository : IRepository
    {
        private readonly List<EventBase> _givenEvents;

        public InMemoryEventRepository(List<EventBase> givenEvents)
        {
            this._givenEvents = givenEvents;
        }

        public List<EventBase> Events { get; private set; }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(id, 0);
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>("BucketDefault", id, 0);
        }

        public TAggregate GetById<TAggregate>(string bucketId, Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(bucketId, id, 0);
        }

        public TAggregate GetById<TAggregate>(string bucketId, Guid id, int version)
            where TAggregate : class, IAggregate
        {
            var aggregate = EventStoreRepository.ConstructAggregate<TAggregate>();
            this._givenEvents.ForEach(aggregate.ApplyEvent);

            return aggregate;
        }

        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            this.Save("BucketDefault", aggregate, commitId, updateHeaders);
        }

        public void Save(string bucketId, IAggregate aggregate, Guid commitId,
            Action<IDictionary<string, object>> updateHeaders)
        {
            this.Events = aggregate.GetUncommittedEvents().Cast<EventBase>().ToList();
        }

        public void Dispose()
        {
            // no op
        }
    }
}
