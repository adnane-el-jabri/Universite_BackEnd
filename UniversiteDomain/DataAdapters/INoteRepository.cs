using UniversiteDomain.Entities;
using System.Linq.Expressions;

namespace UniversiteDomain.DataAdapters;

public interface INoteRepository : IRepository<Note>
{
    Task<Note?> FindByEtudiantAndUeAsync(long etudiantId, long ueId);
}