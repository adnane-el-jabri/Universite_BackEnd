using System.Linq.Expressions;
using UniversiteDomain.Entities;
 
namespace UniversiteDomain.DataAdapters;
 
public interface IEtudiantRepository 
{
    Task<Etudiant> CreateAsync(Etudiant entity);
    Task UpdateAsync(Etudiant entity);
    Task DeleteAsync(long id);
    Task DeleteAsync(Etudiant entity);
    Task<Etudiant?> FindAsync(long id);
    Task<Etudiant?> FindAsync(params object[] keyValues);
    Task<List<Etudiant>> FindByConditionAsync(Expression<Func<Etudiant, bool>> condition);
    Task<List<Etudiant>> FindAllAsync();
    Task SaveChangesAsync();
}