using Microsoft.EntityFrameworkCore;
using UniversiteDomain.Entities;
using UniversiteDomain.DataAdapters;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{
    public async Task<Ue> AssignParcoursAsync(long idUe, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Parcours);

        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Parcours parcours = (await Context.Parcours.FindAsync(idParcours))!;

        if (ue.EnseigneeDans == null)
            ue.EnseigneeDans = new List<Parcours>();

        ue.EnseigneeDans.Add(parcours);
        await Context.SaveChangesAsync();
        return ue;
    }

    public async Task<Ue> AssignParcoursAsync(long idUe, long[] idParcours)
    {
        foreach (var parcoursId in idParcours)
        {
            await AssignParcoursAsync(idUe, parcoursId);
        }
        return (await Context.Ues.FindAsync(idUe))!;
    }
    public async Task<List<Ue>> GetAllAsync()
    {
        return await Context.Ues.ToListAsync();
    }

    public async Task<Ue> GetByIdAsync(long id)
    {
        return await Context.Ues.FindAsync(id);
    }
}