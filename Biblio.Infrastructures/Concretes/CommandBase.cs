using System;
using Biblio.Infrastructures.Abstracts;
using Biblio.Shared.ValueObjects;

namespace Biblio.Infrastructures.Concretes
{
    [Serializable]
    public abstract class CommandBase : MessageBase
    {
        public MessageId MessageId { get; protected set; }
    }
}
