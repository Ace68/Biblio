using System;
using Biblio.Domain.Entities;
using Biblio.Infrastructures.Abstracts;
using Biblio.Messages.Commands;
using Biblio.Shared.ValueObjects;
using CommonDomain.Persistence;

namespace Biblio.Domain.CommandsHandlers
{
    public class BookCommandsHandler : CommandsHandler,
        ICommandHandler<CreateBook>
    {
        public BookCommandsHandler(IRepository biblioRepository) : base(biblioRepository)
        {
        }

        public void Handle(CreateBook command)
        {
            var book = Book.CreateBook(new BookId(command.AggregateId), command.Titolo, command.Autore, command.Isbn);

            this.BiblioRepository.Save(book, Guid.NewGuid(), d => {});
        }
    }
}