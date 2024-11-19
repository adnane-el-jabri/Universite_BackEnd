using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.ParcoursExceptions;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;

namespace Universite.Domain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IParcoursRepository parcoursRepository)
{
    public async Task<Parcours> ExecuteAsync(int id_parcours, string nom, int année_parcours)
    {
        var parcours = new Parcours(){Id = id_parcours, NomParcours = nom, AnneeFormation = année_parcours};
        return await ExecuteAsync(parcours);
    }

    private async Task CheckBusinessRules(Parcours parcours)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(parcours.NomParcours);
        ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
        ArgumentNullException.ThrowIfNull(parcours.Id);
        
        // On recherche un étudiant avec le même numéro étudiant
        List<Parcours> existe = await parcoursRepository.FindByConditionAsync(e=>e.NomParcours.Equals(parcours.NomParcours));
        List<Parcours> parcour = await parcoursRepository.FindByConditionAsync(e=>e.AnneeFormation.Equals(parcours.AnneeFormation));
        // Si un étudiant avec le même numéro étudiant existe déjà, on lève une exception personnalisée
        if (existe .Any()&&parcour.Any()) throw new DuplicateNomParcoursException(parcours.NomParcours+ " - ce  parcours deja existant");
        
        
        if (parcours.NomParcours.Length < 3) throw new InvalidNomEtudiantException(parcours.NomParcours +" incorrect - Le nom d'un parcours doit contenir plus de 3 caractères");
    }

    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours et = await parcoursRepository.CreateAsync(parcours);
        parcoursRepository.SaveChangesAsync().Wait();
        return et;
    }
}

internal class DuplicateNomParcoursException : Exception
{
    public DuplicateNomParcoursException(string ceParcoursDejaExistant)
    {
        throw new NotImplementedException();
    }
}