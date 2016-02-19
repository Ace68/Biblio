using System;

namespace Biblio.EventStore.Persistence.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public readonly Guid Id;
        public readonly Type Type;

        public AggregateNotFoundException(Guid id, Type type)
            : base($"Aggregate '{id}' (type {type.Name}) was not found.")
        {
            this.Id = id;
            this.Type = type;
        }
    }
}
