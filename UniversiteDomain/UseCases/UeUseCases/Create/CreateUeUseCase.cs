using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IUeRepository ueRepository)
{
    public async Task<Ue> ExecuteAsync(string numeroUe, string intitule)
    {
        var ue = new Ue { NumeroUe = numeroUe, Intitule = intitule };
        return await ExecuteAsync(ue);
    }

    public async Task<Ue> ExecuteAsync(Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue newUe = await ueRepository.CreateAsync(ue);
        await ueRepository.SaveChangesAsync();
        return newUe;
    }

    private async Task CheckBusinessRules(Ue ue)
    {
        ArgumentNullException.ThrowIfNull(ue);
        ArgumentNullException.ThrowIfNull(ue.NumeroUe);
        ArgumentNullException.ThrowIfNull(ue.Intitule);

        // Vérifier si le numéro UE est unique
        var existingUe = await ueRepository.FindByConditionAsync(u => u.NumeroUe == ue.NumeroUe);
        if (existingUe.Count > 0)
            throw new DuplicateUeException(ue.NumeroUe + " - ce numéro est déjà utilisé.");

        // Vérifier que l'intitulé a plus de 3 caractères
        if (ue.Intitule.Length < 3)
            throw new InvalidUeIntituleException("L'intitulé de l'UE doit contenir plus de 3 caractères.");
    }
}