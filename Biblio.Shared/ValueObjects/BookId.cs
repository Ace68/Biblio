using System;
using System.Collections.Generic;

namespace Biblio.Shared.ValueObjects
{
    public class BookId : ValueObjectBase<BookId>
    {
        public readonly Guid Id;

        public BookId(Guid bookGuid)
        {
            this.Id = bookGuid;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>();
        }
    }
}