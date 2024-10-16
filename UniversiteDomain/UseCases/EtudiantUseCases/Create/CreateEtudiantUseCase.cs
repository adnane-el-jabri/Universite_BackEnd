using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Create;

public class CreateEtudiantUseCase(IEtudiantRepository etudiantRepository)
{
    public async Task<Etudiant> ExecuteAsync(string numEtud, string nom, string prenom, string email)
    {
        var etudiant = new Etudiant{NumEtud = numEtud, Nom = nom, Prenom = prenom, Email = email};
        return await ExecuteAsync(etudiant);
    }
    public async Task<Etudiant> ExecuteAsync(Etudiant etudiant)
    {
        await CheckBusinessRules(etudiant);
        Etudiant et = await etudiantRepository.CreateAsync(etudiant);
        etudiantRepository.SaveChangesAsync().Wait();
        return et;
    }
    private async Task CheckBusinessRules(Etudiant etudiant)
    {
        // à venir
    }
}