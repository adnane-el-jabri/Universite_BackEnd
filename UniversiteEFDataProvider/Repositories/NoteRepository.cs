using Microsoft.EntityFrameworkCore;
using UniversiteDomain.Entities;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NoteRepository(UniversiteDbContext context) : Repository<Note>(context), INoteRepository
{
    public async Task<Note> AddNoteAsync(long idEtudiant, long idUe, float valeur)
    {
        ArgumentNullException.ThrowIfNull(Context.Notes);
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Ues);

        Etudiant etudiant = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        
        if (valeur < 0 || valeur > 20)
            throw new ArgumentOutOfRangeException("La note doit être comprise entre 0 et 20.");

        if (!etudiant.ParcoursSuivi.UesEnseignees.Contains(ue))
            throw new InvalidOperationException("L'étudiant ne peut avoir une note que dans une UE de son parcours.");

        Note note = new() { EtudiantId = idEtudiant, UeId = idUe, Valeur = valeur };
        await Context.Notes.AddAsync(note);
        await Context.SaveChangesAsync();
        return note;
    }

    public async Task<Note> UpdateNoteAsync(long idEtudiant, long idUe, float valeur)
    {
        Note note = (await Context.Notes.FindAsync(idEtudiant, idUe))!;
        if (note == null) throw new KeyNotFoundException("Note non trouvée.");

        if (valeur < 0 || valeur > 20)
            throw new ArgumentOutOfRangeException("La note doit être comprise entre 0 et 20.");

        note.Valeur = valeur;
        await Context.SaveChangesAsync();
        return note;
    }
    public async Task<Note?> FindByEtudiantAndUeAsync(long etudiantId, long ueId)
    {
        return await Context.Notes!
            .FirstOrDefaultAsync(n => n.EtudiantId == etudiantId && n.UeId == ueId);
    }
    public async Task<List<Note>> GetNotesByUeIdAsync(long ueId)
    {
        return await context.Notes
            .Where(n => n.UeId == ueId)
            .ToListAsync();
    }

    // Ajouter ou mettre à jour une liste de notes
    public async Task AddOrUpdateNotesAsync(List<Note> notes)
    {
        foreach (var note in notes)
        {
            var existingNote = await context.Notes
                .FirstOrDefaultAsync(n => n.EtudiantId == note.EtudiantId && n.UeId == note.UeId);

            if (existingNote != null)
            {
                existingNote.Valeur = note.Valeur; // Mise à jour de la note
            }
            else
            {
                await context.Notes.AddAsync(note);
            }
        }
        await context.SaveChangesAsync();
    }

    // Sauvegarder ou mettre à jour une note unique
    public async Task SaveOrUpdateAsync(Note note)
    {
        var existingNote = await context.Notes
            .FirstOrDefaultAsync(n => n.EtudiantId == note.EtudiantId && n.UeId == note.UeId);

        if (existingNote != null)
        {
            existingNote.Valeur = note.Valeur;
        }
        else
        {
            await context.Notes.AddAsync(note);
        }
        await context.SaveChangesAsync();
    }

}