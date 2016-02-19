using System;
using System.Collections.Generic;
using Biblio.Shared.Exceptions;

namespace Biblio.Shared.ValueObjects
{
    public class MessageId : ValueObjectBase<MessageId>
    {
        public readonly Guid Id;

        public MessageId(Guid messageGuid)
        {
            if (messageGuid == Guid.Empty)
                throw new ArgumentNullException(nameof(messageGuid), DomainExceptions.MessageIdNullException);

            this.Id = messageGuid;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>();
        }
    }
}
