using UniversiteDomain.Exceptions.NoteExceptions;

namespace UniversiteDomain.Entities;


public class Note
{
    public long Id { get; set; }
    public float Valeur { get; set; } // La note (0-20)

    // Clés étrangères
    public long EtudiantId { get; set; }
    public Etudiant? Etudiant { get; set; }

    public long UeId { get; set; }
    public Ue? Ue { get; set; }

    // Vérification des règles métier
    public void CheckBusinessRules()
    {
        if (Valeur < 0 || Valeur > 20)
            throw new InvalidException ("La note doit être comprise entre 0 et 20");

        if (Etudiant == null || Ue == null)
            throw new ArgumentNullException("L'étudiant et l'UE doivent être renseignés");

        if (Etudiant.ParcoursSuivi == null || !Etudiant.ParcoursSuivi.UesEnseignees.Contains(Ue))
            throw new InvalidException ("L'étudiant ne peut avoir une note que dans une UE de son parcours");
    }
}