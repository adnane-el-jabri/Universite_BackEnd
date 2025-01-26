namespace UniversiteEFDataProvider.RepositoryFactories;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Repositories;
using UniversiteDomain.Entities;


public class RepositoryFactory (UniversiteDbContext context): IRepositoryFactory
{
    private IParcoursRepository? _parcours;
    private IEtudiantRepository? _etudiants;
    private IUeRepository? _ues;
    private INoteRepository? _notes;
    
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