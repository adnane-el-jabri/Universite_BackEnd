using UniversiteDomain.Entities;
using System.Linq.Expressions;

namespace UniversiteDomain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
    Task<Note?> FindByEtudiantAndUeAsync(long etudiantId, long ueId);
    Task<List<Note>> GetNotesByUeIdAsync(long ueId);
    Task AddOrUpdateNotesAsync(List<Note> notes);
    Task SaveOrUpdateAsync(Note note);
}