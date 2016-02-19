using System;
using Biblio.Infrastructures.Abstracts;

namespace Biblio.Infrastructures.Concretes
{
    [Serializable]
    public abstract class EventBase : MessageBase
    {
        public Guid MessageGuid { get; protected set; }
    }
}
