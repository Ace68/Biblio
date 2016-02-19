using Biblio.Infrastructures.Concretes;

namespace Biblio.Infrastructures.Abstracts
{
    public interface ICommandHandler<in T> where T : CommandBase
    {
        void Handle(T command);
    }
}
