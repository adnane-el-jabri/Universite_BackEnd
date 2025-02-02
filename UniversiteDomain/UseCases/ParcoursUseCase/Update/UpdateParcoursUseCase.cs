using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Update
{
    public class UpdateParcoursUseCase
    {
        private readonly IParcoursRepository _parcoursRepository;

        public UpdateParcoursUseCase(IRepositoryFactory repositoryFactory)
        {
            _parcoursRepository = repositoryFactory.ParcoursRepository();
        }

        public async Task ExecuteAsync(Parcours parcours)
        {
            await _parcoursRepository.UpdateAsync(parcours);
        }
        public bool IsAuthorized(string role)
        {
            // Autoriser uniquement les rôles 'Responsable' et 'Scolarite' à créer un parcours
            return role == "Responsable" || role == "Scolarite";
        }
    }
}