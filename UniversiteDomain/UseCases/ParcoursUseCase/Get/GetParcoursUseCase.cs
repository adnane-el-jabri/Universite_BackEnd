using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Get
{
    public class GetParcoursUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public GetParcoursUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Récupère tous les parcours.
        /// </summary>
        public async Task<List<Parcours>> ExecuteAsync()
        {
            var parcoursRepo = _repositoryFactory.ParcoursRepository();
            return await parcoursRepo.GetAllAsync();
        }

        /// <summary>
        /// Récupère un parcours par son ID.
        /// </summary>
        public async Task<Parcours?> ExecuteAsync(long id)
        {
            var parcoursRepo = _repositoryFactory.ParcoursRepository();
            return await parcoursRepo.GetByIdAsync(id);
        }

        /// <summary>
        /// Vérifie si l'utilisateur est autorisé à accéder aux informations du parcours.
        /// </summary>
        public bool IsAuthorized(string role)
        {
            return role == Roles.Responsable || role == Roles.Scolarite;
        }
    }
}