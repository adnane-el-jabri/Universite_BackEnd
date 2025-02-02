using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Get
{
    public class GetUeUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public GetUeUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<Ue>> ExecuteAsync()
        {
            return await _repositoryFactory.UeRepository().GetAllAsync();
        }

        public async Task<Ue> ExecuteByIdAsync(long id)
        {
            return await _repositoryFactory.UeRepository().GetByIdAsync(id);
        }

        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }
    }
}