using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Update
{
    public class UpdateUeUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public UpdateUeUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task ExecuteAsync(Ue ue)
        {
            await _repositoryFactory.UeRepository().UpdateAsync(ue);
        }

        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }
    }
}