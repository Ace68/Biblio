using System;
using Biblio.Infrastructures.Concretes;

namespace Biblio.Messages.Events
{
    public class BookCreated : EventBase
    {
        public readonly string Titolo;
        public readonly string Autore;
        public readonly string Isbn;

        public BookCreated(Guid bookGuid, string titolo, string autore, string isbn)
        {
            this.AggregateId = bookGuid;
            this.Titolo = titolo;
            this.Autore = autore;
            this.Isbn = isbn;
        }
    }
}