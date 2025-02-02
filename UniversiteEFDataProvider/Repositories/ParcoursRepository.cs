using Microsoft.EntityFrameworkCore;
using UniversiteDomain.Entities;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        if (parcours == null) throw new ArgumentNullException(nameof(parcours));
        if (etudiant == null) throw new ArgumentNullException(nameof(etudiant));

        var existingParcours = await Context.Parcours
            .Include(p => p.Inscrits)
            .FirstOrDefaultAsync(p => p.Id == parcours.Id);

        if (existingParcours == null)
            throw new KeyNotFoundException("Parcours non trouvé.");

        if (existingParcours.Inscrits == null)
            existingParcours.Inscrits = new List<Etudiant>();

        existingParcours.Inscrits.Add(etudiant);
        await Context.SaveChangesAsync();
        return existingParcours;
    }

    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        var parcours = await Context.Parcours.FindAsync(idParcours);
        var etudiant = await Context.Etudiants.FindAsync(idEtudiant);

        if (parcours == null)
            throw new KeyNotFoundException($"Parcours avec ID {idParcours} non trouvé.");
        if (etudiant == null)
            throw new KeyNotFoundException($"Étudiant avec ID {idEtudiant} non trouvé.");

        return await AddEtudiantAsync(parcours, etudiant);
    }

    public async Task<Parcours> AddEtudiantAsync(Parcours? parcours, List<Etudiant> etudiants)
    {
        if (parcours == null) throw new ArgumentNullException(nameof(parcours));
        if (etudiants == null || etudiants.Count == 0) throw new ArgumentNullException(nameof(etudiants));

        var existingParcours = await Context.Parcours
            .Include(p => p.Inscrits)
            .FirstOrDefaultAsync(p => p.Id == parcours.Id);

        if (existingParcours == null)
            throw new KeyNotFoundException("Parcours non trouvé.");

        if (existingParcours.Inscrits == null)
            existingParcours.Inscrits = new List<Etudiant>();

        existingParcours.Inscrits.AddRange(etudiants);
        await Context.SaveChangesAsync();
        return existingParcours;
    }

    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        var parcours = await Context.Parcours.FindAsync(idParcours);
        if (parcours == null)
            throw new KeyNotFoundException($"Parcours avec ID {idParcours} non trouvé.");

        var etudiants = await Context.Etudiants
            .Where(e => idEtudiants.Contains(e.Id))
            .ToListAsync();

        return await AddEtudiantAsync(parcours, etudiants);
    }
    public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
    {
        var parcours = await Context.Parcours.Include(p => p.UesEnseignees).FirstOrDefaultAsync(p => p.Id == idParcours);
        var ue = await Context.Ues.FindAsync(idUe);

        if (parcours == null)
            throw new KeyNotFoundException($"Parcours avec ID {idParcours} non trouvé.");
        if (ue == null)
            throw new KeyNotFoundException($"UE avec ID {idUe} non trouvée.");

        if (parcours.UesEnseignees == null)
            parcours.UesEnseignees = new List<Ue>();

        if (!parcours.UesEnseignees.Contains(ue))
        {
            parcours.UesEnseignees.Add(ue);
            await Context.SaveChangesAsync();
        }

        return parcours;
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        var parcours = await Context.Parcours.Include(p => p.UesEnseignees).FirstOrDefaultAsync(p => p.Id == idParcours);
        if (parcours == null)
            throw new KeyNotFoundException($"Parcours avec ID {idParcours} non trouvé.");

        var ues = await Context.Ues.Where(ue => idUes.Contains(ue.Id)).ToListAsync();
    
        if (ues.Count == 0)
            throw new KeyNotFoundException("Aucune UE valide trouvée.");

        if (parcours.UesEnseignees == null)
            parcours.UesEnseignees = new List<Ue>();

        foreach (var ue in ues)
        {
            if (!parcours.UesEnseignees.Contains(ue))
            {
                parcours.UesEnseignees.Add(ue);
            }
        }

        await Context.SaveChangesAsync();
        return parcours;
    }
    public async Task<List<Parcours>> GetAllAsync()
    {
        return await Context.Parcours.ToListAsync();
    }

    public async Task<Parcours> GetByIdAsync(long id)
    {
        return await Context.Parcours.FindAsync(id);
    }

}
