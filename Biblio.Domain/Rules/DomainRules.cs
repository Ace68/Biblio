using System;
using Biblio.Shared.Exceptions;
using Biblio.Shared.ValueObjects;

namespace Biblio.Domain.Rules
{
    internal static class DomainRules
    {
        #region Book
        internal static void ChkBookId(BookId bookId)
        {
            if (bookId.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(bookId), DomainExceptions.BookIdNullException);
        }

        internal static void ChkTitolo(string titolo)
        {
            if (string.IsNullOrEmpty(titolo))
                throw new ArgumentNullException(nameof(titolo), DomainExceptions.TitoloNullException);
        }
        #endregion
    }
}