using UniversiteDomain.DataAdapters;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Delete
{
    public class DeleteParcoursUseCase
    {
        private readonly IParcoursRepository _parcoursRepository;

        public DeleteParcoursUseCase(IRepositoryFactory repositoryFactory)
        {
            _parcoursRepository = repositoryFactory.ParcoursRepository();
        }

        public async Task ExecuteAsync(long id)
        {
            await _parcoursRepository.DeleteAsync(id);
        }
        public bool IsAuthorized(string role)
        {
            // Autoriser uniquement les rôles 'Responsable' et 'Scolarite' à créer un parcours
            return role == "Responsable" || role == "Scolarite";
        }
    }
}