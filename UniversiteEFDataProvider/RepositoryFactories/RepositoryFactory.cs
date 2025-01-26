namespace UniversiteEFDataProvider.RepositoryFactories;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Repositories;
using UniversiteDomain.Entities;
using Microsoft.AspNetCore.Identity;
using UniversiteEFDataProvider.Entities;

public class RepositoryFactory (UniversiteDbContext context, 
                                UserManager<UniversiteUser> userManager, 
                                RoleManager<UniversiteRole> roleManager) 
    : IRepositoryFactory
{
    private IParcoursRepository? _parcours;
    private IEtudiantRepository? _etudiants;
    private IUeRepository? _ues;
    private INoteRepository? _notes;
    
    // Ajout des nouveaux repositories
    private IUniversiteRoleRepository? _universiteRole;
    private IUniversiteUserRepository? _universiteUser;
    
    public IParcoursRepository ParcoursRepository()
    {
        if (_parcours == null)
        {
            _parcours = new ParcoursRepository(context ?? throw new InvalidOperationException());
        }
        return _parcours;
    }

    public IEtudiantRepository EtudiantRepository()
    {
        if (_etudiants == null)
        {
            _etudiants = new EtudiantRepository(context ?? throw new InvalidOperationException());
        }
        return _etudiants;
    }

    public IUeRepository UeRepository()
    {
        if (_ues == null)
        {
            _ues = new UeRepository(context ?? throw new InvalidOperationException());
        }
        return _ues;
    }

    public INoteRepository NoteRepository()
    {
        if (_notes == null)
        {
            _notes = new NoteRepository(context ?? throw new InvalidOperationException());
        }
        return _notes;
    }
    
    // 🔹 Ajout du Repository pour les rôles
    public IUniversiteRoleRepository UniversiteRoleRepository()
    {
        if (_universiteRole == null)
        {
            _universiteRole = new UniversiteRoleRepository(context ?? throw new InvalidOperationException(), roleManager);
        }
        return _universiteRole;
    }

    // 🔹 Ajout du Repository pour les utilisateurs
    public IUniversiteUserRepository UniversiteUserRepository()
    {
        if (_universiteUser == null)
        {
            _universiteUser = new UniversiteUserRepository(context ?? throw new InvalidOperationException(), userManager, roleManager);
        }
        return _universiteUser;
    }
       
    public async Task SaveChangesAsync()
    {
        context.SaveChangesAsync().Wait();
    }

    public Task<Etudiant> CreateAsync(Etudiant etudiant)
    {
        throw new NotImplementedException();
    }

    public async Task EnsureCreatedAsync()
    {
        context.Database.EnsureCreated();
    }
    public async Task EnsureDeletedAsync()
    {
        context.Database.EnsureDeleted();
    }
}
