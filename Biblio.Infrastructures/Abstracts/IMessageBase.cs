using System;

namespace Biblio.Infrastructures.Abstracts
{
    public interface IMessageBase
    {
        Guid AggregateId { get; set; }
        int Version { get; set; }
    }
}
