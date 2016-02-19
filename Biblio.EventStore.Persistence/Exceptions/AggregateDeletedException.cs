using System;

namespace Biblio.EventStore.Persistence.Exceptions
{
    public class AggregateDeletedException : Exception
    {
        public readonly Guid Id;
        public readonly Type Type;

        public AggregateDeletedException(Guid id, Type type)
            : base($"Aggregate '{id}' (type {type.Name}) was deleted.")
        {
            this.Id = id;
            this.Type = type;
        }
    }
}
