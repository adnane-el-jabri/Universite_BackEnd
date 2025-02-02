using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IUeRepository : IRepository<Ue>
{
    Task<List<Ue>> GetAllAsync();
    Task<Ue> GetByIdAsync(long id);
}