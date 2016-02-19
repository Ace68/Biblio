using Biblio.Domain.Abstracts;
using Biblio.Domain.Rules;
using Biblio.Messages.Events;
using Biblio.Shared.ValueObjects;

namespace Biblio.Domain.Entities
{
    public class Book : AggregateRoot
    {
        private string _titolo;
        private string _autore;
        private string _isbn;

        protected Book()
        { }

        #region ctor

        internal static Book CreateBook(BookId bookId, string titolo, string autore, string isbn)
        {
            DomainRules.ChkBookId(bookId);
            DomainRules.ChkTitolo(titolo);

            var book = new Book(bookId, titolo, autore, isbn);

            return book;

        }

        private Book(BookId bookId, string titolo, string autore, string isbn)
        {
            this.RaiseEvent(new BookCreated(bookId.Id, titolo, autore, isbn));
        }

        private void Apply(BookCreated @event)
        {
            this.Id = @event.AggregateId;
            this._autore = @event.Autore;
            this._titolo = @event.Titolo;
            this._isbn = @event.Isbn;
        }
        #endregion
    }
}