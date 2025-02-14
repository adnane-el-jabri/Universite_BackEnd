namespace UniversiteEFDataProvider.Repositories;
using Microsoft.EntityFrameworkCore;
using UniversiteDomain.Entities;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;



public class EtudiantRepository(UniversiteDbContext context) : Repository<Etudiant>(context), IEtudiantRepository
{
    public async Task AffecterParcoursAsync(long idEtudiant, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        e.ParcoursSuivi = p;
        await Context.SaveChangesAsync();
    }
    
    public async Task AffecterParcoursAsync(Etudiant etudiant, Parcours parcours)
    {
        await AffecterParcoursAsync(etudiant.Id, parcours.Id); 
    }

    public async Task<Etudiant> GetByIdAsync(long id)
    {
        return await context.Etudiants.FindAsync(id);
    }

    
    public async Task<Etudiant?> FindEtudiantCompletAsync(long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        return await Context.Etudiants.Include(e => e.Notes).ThenInclude(n=>n.Ue).FirstOrDefaultAsync(e => e.Id == idEtudiant);
    }
    public async Task<List<Etudiant>> GetAllAsync()
    {
        return await Context.Etudiants.ToListAsync();
    }
    
    public async Task<List<Etudiant>> GetEtudiantsByUeIdAsync(long ueId)
    {
        return await Context.Etudiants
            .Where(e => Context.Notes.Any(n => n.EtudiantId == e.Id && n.UeId == ueId))
            .ToListAsync();
    }

}