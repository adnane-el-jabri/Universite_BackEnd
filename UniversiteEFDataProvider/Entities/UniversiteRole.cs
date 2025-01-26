using Microsoft.AspNetCore.Identity;
using UniversiteDomain.Entities;

namespace UniversiteEFDataProvider.Entities;

public class UniversiteRole: IdentityRole, IUniversiteRole
{
    private string _role = string.Empty;
    
    public UniversiteRole() : base()
    {
    }
    public UniversiteRole(string role) : base(role)
    {
        _role = role;
    }}