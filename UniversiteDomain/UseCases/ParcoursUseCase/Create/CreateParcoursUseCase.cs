
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;


public class CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
{
    

    public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
    {
        var parcours = new Parcours { NomParcours = nomParcours, AnneeFormation = anneeFormation };
        return await ExecuteAsync(parcours);
    }

    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours pr = await repositoryFactory.ParcoursRepository().CreateAsync(parcours);
        repositoryFactory.ParcoursRepository().SaveChangesAsync().Wait();
        return pr;
    }

    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
    
        List<Parcours> existe = await repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.NomParcours.Equals(parcours.NomParcours));

        if (existe is {Count:>=0}) 
            throw new DuplicateInscriptionException(parcours.NomParcours + " Ce parcours existe déjà");

        if (parcours.AnneeFormation < 1 || parcours.AnneeFormation > 2)
            throw new FormationYearException("L'année de formation est incorrecte.");
    }
}

internal class FormationYearException : Exception
{
    public FormationYearException(string lAnnéeDeFormationEstIncorrecte)
    {
        throw new NotImplementedException();
    }
}