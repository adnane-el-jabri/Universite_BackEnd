using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Delete
{
    public class DeleteUeUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public DeleteUeUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task ExecuteAsync(long id)
        {
            await _repositoryFactory.UeRepository().DeleteAsync(id);
        }

        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }
    }
}