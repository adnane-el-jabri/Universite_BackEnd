using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IUniversiteRoleRepository : IRepository<IUniversiteRole>
{
    Task AddRoleAsync(string role);
}