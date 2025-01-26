using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Get;

public class GetEtudiantCompletUseCase(IRepositoryFactory factory)
{
    public async Task<Etudiant?> ExecuteAsync(long idEtudiant)
    {
        await CheckBusinessRules();
        Etudiant? etudiant = await factory.EtudiantRepository().FindEtudiantCompletAsync(idEtudiant);
        return etudiant;
    }

    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IEtudiantRepository etudiantRepository = factory.EtudiantRepository();
        ArgumentNullException.ThrowIfNull(etudiantRepository);
    }

    public bool IsAuthorized(string role, IUniversiteUser user, long idEtudiant)
    {
        if (role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable)) return true;
        return user.Etudiant != null && role.Equals(Roles.Etudiant) && user.Etudiant.Id == idEtudiant;
    }
}