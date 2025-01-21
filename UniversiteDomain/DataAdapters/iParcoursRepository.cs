using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IParcoursRepository : IRepository<Parcours>
{
    Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant);
    Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant);
    Task<Parcours> AddEtudiantAsync(Parcours ? parcours, List<Etudiant> etudiants);
    Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants);
    // Ajouter une Ue à un parcours
    Task<Parcours> AddUeAsync(long idParcours, long idUe);

    // Ajouter plusieurs UEs à un parcours
    Task<Parcours> AddUeAsync(long idParcours, long[] idUes);
    
}