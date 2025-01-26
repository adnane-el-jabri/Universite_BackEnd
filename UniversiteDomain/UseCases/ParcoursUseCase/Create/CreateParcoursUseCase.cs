using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class CreateParcoursUseCase
{
    private readonly IRepositoryFactory _repositoryFactory;

    public CreateParcoursUseCase(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public async Task<Parcours> ExecuteAsync(string nomParcours, int anneeFormation)
    {
        var parcours = new Parcours { NomParcours = nomParcours, AnneeFormation = anneeFormation };
        return await ExecuteAsync(parcours);
    }

    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        
        // Création du parcours
        Parcours pr = await _repositoryFactory.ParcoursRepository().CreateAsync(parcours);
        
        // Sauvegarde de la base de données
        await _repositoryFactory.SaveChangesAsync();
        
        return pr;
    }

    private async Task CheckBusinessRules(Parcours parcours)
    {
        if (parcours == null) throw new ArgumentNullException(nameof(parcours));
        if (string.IsNullOrWhiteSpace(parcours.NomParcours)) throw new ArgumentNullException(nameof(parcours.NomParcours));

        // Vérifier si le parcours existe déjà
        var existe = await _repositoryFactory.ParcoursRepository().FindByConditionAsync(p => p.NomParcours.Equals(parcours.NomParcours));

        if (existe is { Count: > 0 }) 
            throw new DuplicateInscriptionException($"{parcours.NomParcours} Ce parcours existe déjà");

        // Vérifier si l'année de formation est correcte
        if (parcours.AnneeFormation < 1 || parcours.AnneeFormation > 2)
            throw new FormationYearException("L'année de formation est incorrecte.");
    }
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }

}

// Exception correctement définie
internal class FormationYearException : Exception
{
    public FormationYearException(string message) : base(message) { }
}
