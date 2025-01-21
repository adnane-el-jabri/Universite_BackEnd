using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases;

public class AddNoteUseCase(INoteRepository noteRepository, IEtudiantRepository etudiantRepository, IUeRepository ueRepository)
{
    public async Task<Note> ExecuteAsync(long etudiantId, long ueId, float valeur)
    {
        // Vérification des entrées
        if (valeur < 0 || valeur > 20)
            throw new InvalidNoteException("La note doit être comprise entre 0 et 20");

        // Vérification de l'existence de l'étudiant et de l'UE
        var etudiant = await etudiantRepository.FindAsync(etudiantId);
        var ue = await ueRepository.FindAsync(ueId);

        if (etudiant == null) throw new EtudiantNotFoundException($"Étudiant {etudiantId} introuvable.");
        if (ue == null) throw new UeNotFoundException($"UE {ueId} introuvable.");

        // Vérification de l'inscription de l'étudiant au parcours de l'UE
        if (etudiant.ParcoursSuivi == null || !etudiant.ParcoursSuivi.UesEnseignees.Contains(ue))
            throw new InvalidNoteException("L'étudiant ne peut pas être noté pour une UE hors de son parcours.");

        // Vérification si une note existe déjà
        var existingNote = await noteRepository.FindByEtudiantAndUeAsync(etudiantId, ueId);
        if (existingNote != null)
            throw new DuplicateNoteException($"L'étudiant {etudiantId} a déjà une note dans l'UE {ueId}.");

        // Création et enregistrement de la nouvelle note
        var note = new Note { Valeur = valeur, Etudiant = etudiant, Ue = ue };
        note.CheckBusinessRules();

        await noteRepository.CreateAsync(note);
        await noteRepository.SaveChangesAsync();

        return note;
    }
}