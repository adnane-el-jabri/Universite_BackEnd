using System.Linq.Expressions;
using UniversiteDomain.Entities;
 
namespace UniversiteDomain.DataAdapters;
 
public interface IEtudiantRepository : IRepository<Etudiant>
{
    new Task<Etudiant> CreateAsync(Etudiant entity);
    new Task UpdateAsync(Etudiant etudiant);
    new Task DeleteAsync(long id);
    new Task DeleteAsync(Etudiant entity);
    new Task<Etudiant?> FindAsync(long id);
    new Task<Etudiant?> FindAsync(params object[] keyValues);
    new Task<List<Etudiant>> FindByConditionAsync(Expression<Func<Etudiant, bool>> condition);
    new Task<List<Etudiant>> FindAllAsync();
    Task<Etudiant> GetByIdAsync(long id);
    new Task SaveChangesAsync();
    Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant);
   
}