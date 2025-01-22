using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;

public class AddUeDansParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    // Ajouter une UE dans un parcours
    public async Task<Parcours> ExecuteAsync(Parcours parcours, Ue ue)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(ue);
        return await ExecuteAsync(parcours.Id, ue.Id);
    }

    public async Task<Parcours> ExecuteAsync(long idParcours, long idUe)
    {
        await CheckBusinessRules(idParcours, idUe);
        return await repositoryFactory.ParcoursRepository().AddUeAsync(idParcours, idUe);
    }

    // Ajouter plusieurs UEs dans un parcours
    public async Task<Parcours> ExecuteAsync(Parcours parcours, List<Ue> ues)
    {
        ArgumentNullException.ThrowIfNull(ues);
        ArgumentNullException.ThrowIfNull(parcours);
        long[] idUes = ues.Select(x => x.Id).ToArray();
        return await ExecuteAsync(parcours.Id, idUes);
    }

    public async Task<Parcours> ExecuteAsync(long idParcours, long[] idUes)
    {
        foreach (var id in idUes)
            await CheckBusinessRules(idParcours, id);

        return await repositoryFactory.ParcoursRepository().AddUeAsync(idParcours, idUes);
    }

    // Vérification des règles de gestion
    private async Task CheckBusinessRules(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(idParcours);
        ArgumentNullException.ThrowIfNull(idUe);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idParcours);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idUe);

        // Vérification de la connexion aux repositories
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        ArgumentNullException.ThrowIfNull(repositoryFactory.UeRepository());
        ArgumentNullException.ThrowIfNull(repositoryFactory.ParcoursRepository());

        // Vérifier si l'UE existe
        List<Ue> ue = await repositoryFactory.UeRepository().FindByConditionAsync(e => e.Id.Equals(idUe));
        if (ue == null || ue.Count == 0) throw new UeNotFoundException(idUe.ToString());

        // Vérifier si le parcours existe
        List<Parcours> parcours = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.Id.Equals(idParcours));
        if (parcours == null || parcours.Count == 0) throw new ParcoursNotFoundException(idParcours.ToString());

        // Vérifier si l'UE n'est pas déjà ajoutée au parcours
        if (parcours[0].UesEnseignees != null && parcours[0].UesEnseignees.Any(e => e.Id.Equals(idUe)))
        {
            throw new DuplicateUeDansParcoursException($"UE {idUe} est déjà ajoutée au parcours {idParcours}");
        }
    }
}
