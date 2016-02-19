using System;
using System.Collections.Generic;
using Biblio.Domain.CommandsHandlers;
using Biblio.Infrastructures.Abstracts;
using Biblio.Infrastructures.Concretes;
using Biblio.Messages.Commands;
using Biblio.Messages.Events;
using Biblio.Shared.ValueObjects;

namespace Biblio.Domain.Test.Book
{
    public class CreateBookTest : EventSpecification<CreateBook>
    {
        private readonly BookId _bookId = new BookId(Guid.NewGuid());

        private const string Titolo = "Titolo";
        private const string Autore = "Autore";
        private const string ISBN = "ISBN";

        protected override IEnumerable<EventBase> Given()
        {
            yield break;
        }

        protected override CreateBook When()
        {
            return new CreateBook(this._bookId, Titolo, Autore, ISBN);
        }

        protected override ICommandHandler<CreateBook> OnHandler()
        {
            return new BookCommandsHandler(this.Repository);
        }

        protected override IEnumerable<EventBase> Expect()
        {
            yield return new BookCreated(this._bookId.Id, Titolo, Autore, ISBN);
        }
    }
}