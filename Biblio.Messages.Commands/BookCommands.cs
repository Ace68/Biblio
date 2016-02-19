using Biblio.Infrastructures.Concretes;
using Biblio.Shared.ValueObjects;

namespace Biblio.Messages.Commands
{
    public class CreateBook : CommandBase
    {
        public readonly string Titolo;
        public readonly string Autore;
        public readonly string Isbn;

        public CreateBook(BookId bookId, string titolo, string autore, string isbn)
        {
            this.AggregateId = bookId.Id;
            this.Titolo = titolo;
            this.Autore = autore;
            this.Isbn = isbn;
        }
    }
}