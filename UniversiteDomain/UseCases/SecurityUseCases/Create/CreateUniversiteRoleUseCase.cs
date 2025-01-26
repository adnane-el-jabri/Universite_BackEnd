using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.SecurityUseCases.Create;

public class CreateUniversiteRoleUseCase(IRepositoryFactory factory)
{
    public async Task ExecuteAsync(string role)
    {
        await CheckBusinessRules(role);
        await factory.UniversiteRoleRepository().AddRoleAsync(role);
        await factory.SaveChangesAsync();
    }

    private async Task CheckBusinessRules(string role)
    {
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(factory);
    }
    
    public bool IsAuthorized(string role)
    {
        return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
    }
}