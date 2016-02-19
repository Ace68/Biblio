using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biblio.EventStore.Persistence.Exceptions;
using Biblio.Infrastructures.Core;
using CommonDomain;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Biblio.EventStore.Persistence.Repositories
{
    public class EventStoreRepository : IRepository
    {
        private Func<Type, Guid, string> _aggregateIdToStreamName;

        private readonly IEventStoreConnection _eventStoreConnection;
        private static readonly JsonSerializerSettings SerializerSettings;

        static EventStoreRepository()
        {
            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        }

        public EventStoreRepository(IEventStoreConnection eventStoreConnection)
        {
            this._eventStoreConnection = eventStoreConnection;
        }

        //public GetEventStoreRepository(IEventStoreConnection eventStoreConnection)
        //    : this(eventStoreConnection, (t, g) => string.Format("{0}-{1}", char.ToLower(t.Name[0]) + t.Name.Substring(1), g.ToString("N")))
        //{
        //}

        //public GetEventStoreRepository(IEventStoreConnection eventStoreConnection,
        //    Func<Type, Guid, string> aggregateIdToStreamName)
        //{
        //    this._eventStoreConnection = eventStoreConnection;
        //    this._aggregateIdToStreamName = aggregateIdToStreamName;
        //}

        private void StreamNameFactory(IAggregate aggregate, Guid aggregateId)
        {
            this._aggregateIdToStreamName =
                (t, g) =>
                    $"{char.ToLower(aggregate.GetType().Name[0])}{aggregate.GetType().Name.Substring(1)}-{aggregateId.ToString("N")}";
        }

        #region Interfaces Implementation
        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(id, int.MaxValue);
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            if (version <= 0)
                throw new InvalidOperationException("Cannot get version <= 0");

            var aggregate = ConstructAggregate<TAggregate>();
            this.StreamNameFactory(aggregate, id);

            var streamName = this._aggregateIdToStreamName(typeof(TAggregate), id);

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + EventStoreCore.ReadPageSize <= version
                                    ? EventStoreCore.ReadPageSize
                                    : version - sliceStart + 1;

                currentSlice = this._eventStoreConnection.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount,
                    false).Result;

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof(TAggregate));

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof(TAggregate));

                sliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                    aggregate.ApplyEvent(DeserializeEvent(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data));
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            if (aggregate.Version != version && version < Int32.MaxValue)
                throw new AggregateVersionException(id, typeof(TAggregate), aggregate.Version, version);

            return aggregate;
        }

        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            this.StreamNameFactory(aggregate, aggregate.Id);

            var commitHeaders = new Dictionary<string, object>
            {
                { EventStoreCore.CommitIdHeader, commitId },
                { EventStoreCore.AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName }
            };
            updateHeaders(commitHeaders);

            var streamName = this._aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;
            var eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();

            if (eventsToSave.Count < EventStoreCore.WritePageSize)
            {
                this._eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave).Wait();
            }
            else
            {
                var transaction = this._eventStoreConnection.StartTransactionAsync(streamName, expectedVersion).Result;

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(EventStoreCore.WritePageSize);
                    transaction.WriteAsync(pageEvents).Wait();
                    position += EventStoreCore.WritePageSize;
                }

                transaction.CommitAsync().Wait();

                transaction.Dispose();
            }

            aggregate.ClearUncommittedEvents();
        }
        #endregion

        #region Helpers
        internal static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventStoreCore.EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventStoreCore.EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }
        #endregion
    }
}
