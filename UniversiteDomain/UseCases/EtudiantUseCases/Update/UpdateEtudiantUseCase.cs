using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Update
{
    public class UpdateEtudiantUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public UpdateEtudiantUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task ExecuteAsync(Etudiant etudiant)
        {
            var repo = _repositoryFactory.EtudiantRepository();
            var existingEtudiant = await repo.GetByIdAsync(etudiant.Id);
            if (existingEtudiant == null)
            {
                throw new Exception("Étudiant non trouvé.");
            }

            existingEtudiant.NumEtud = etudiant.NumEtud;
            existingEtudiant.Nom = etudiant.Nom;
            existingEtudiant.Prenom = etudiant.Prenom;
            existingEtudiant.Email = etudiant.Email;
            
            await repo.UpdateAsync(existingEtudiant);
        }

        public bool IsAuthorized(string role)
        {
            return role == Roles.Scolarite || role == Roles.Responsable;
        }
    }
}