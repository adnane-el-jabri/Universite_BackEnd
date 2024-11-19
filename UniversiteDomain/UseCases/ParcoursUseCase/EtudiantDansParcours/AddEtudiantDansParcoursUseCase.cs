using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.ParcoursExceptions;

namespace UniversiteDomain.UseCases.ParcoursUseCases.EtudiantDansParcours;

public class AddEtudiantDansParcoursUseCase(IEtudiantRepository _etudiantRepository, IParcoursRepository _parcoursRepository)
{
   
    // Rajout d'un étudiant dans un parcours
    public async Task<Parcours> ExecuteAsync(Parcours parcours, Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(etudiant);
        return await ExecuteAsync(parcours.Id, etudiant.Id);
    }  

    public async Task<Parcours> ExecuteAsync(long idParcours, long idEtudiant)
    {
        await CheckBusinessRules(idParcours, idEtudiant); 
        return await _parcoursRepository.AddEtudiantAsync(idParcours, idEtudiant);
    }

    // Rajout de plusieurs étudiants dans un parcours
    public async Task<Parcours> ExecuteAsync(Parcours parcours, List<Etudiant> etudiants)
    {
        long[] idEtudiants = etudiants.Select(x => x.Id).ToArray();
        return await ExecuteAsync(parcours.Id, idEtudiants); 
    }  

    public async Task<Parcours> ExecuteAsync(long idParcours, long[] idEtudiants)
    {
        foreach (var id in idEtudiants)
        {
            await CheckBusinessRules(idParcours, id);
        }
        return await _parcoursRepository.AddEtudiantAsync(idParcours, idEtudiants);
    }

    private async Task CheckBusinessRules(long idParcours, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(idParcours);
        ArgumentNullException.ThrowIfNull(idEtudiant);
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idParcours);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idEtudiant);
        
        // Vérifions tout d'abord que nous sommes bien connectés aux datasources
        ArgumentNullException.ThrowIfNull(_etudiantRepository);
        ArgumentNullException.ThrowIfNull(_parcoursRepository);
        
        // On recherche l'étudiant
        List<Etudiant> etudiant = await _etudiantRepository.FindByConditionAsync(e=>e.Id.Equals(idEtudiant));;
        if (etudiant is { Count: 0 }) throw new EtudiantNotFoundException(idEtudiant.ToString());
        // On recherche le parcours
        List<Parcours> parcours = await _parcoursRepository.FindByConditionAsync(p=>p.Id.Equals(idParcours));;
        if (parcours is { Count: 0 }) throw new ParcoursNotFoundException(idParcours.ToString());
        
        // On vérifie que l'étudiant n'est pas déjà dans le parcours
        List<Etudiant> inscrit = await _etudiantRepository.FindByConditionAsync(e=>e.Id.Equals(idEtudiant) && e.ParcoursSuivi.Id.Equals(idParcours));
        if (inscrit is { Count: > 0 }) throw new DuplicateInscriptionException(idEtudiant+" est déjà inscrit dans le parcours dans le parcours : "+idParcours);
    }

    private async Task CheckBusinessRules(long idParcours, long idEtudiant, IEnumerable<Etudiant> etudiants)
    {
        // Vérification des paramètres
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idParcours);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idEtudiant);
        
        
        if (etudiants == null)
        {
            throw new ArgumentNullException(nameof(etudiants), "La liste des étudiants ne peut pas être nulle.");
        }
        
        
        
        // On recherche l'étudiant
        var etudiant = await _etudiantRepository.FindByConditionAsync(e => e.Id == idEtudiant);
        if (!etudiant.Any()) throw new EtudiantNotFoundException(idEtudiant.ToString());

        // On recherche le parcours
        var parcours = await _parcoursRepository.FindByConditionAsync(p => p.Id == idParcours);
        if (!parcours.Any()) throw new ParcoursNotFoundException(idParcours.ToString());
        
        // On vérifie que l'étudiant n'est pas déjà dans le parcours
        var inscrit = await _etudiantRepository.FindByConditionAsync(e => e.Id == idEtudiant && e.ParcoursSuivi.Id == idParcours);
        if (inscrit.Any()) throw new DuplicateInscriptionException($"{idEtudiant} est déjà inscrit dans le parcours : {idParcours}");      
    }

}
