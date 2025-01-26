using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete;

public class DeleteEtudiantUseCase
{
    private readonly IRepositoryFactory _repositoryFactory;

    public DeleteEtudiantUseCase(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<bool> ExecuteAsync(long etudiantId)
    {
        var etudiantRepo = _repositoryFactory.EtudiantRepository();

        // Vérifier si l'étudiant existe
        var etudiant = await etudiantRepo.GetByIdAsync(etudiantId);
        if (etudiant == null)
        {
            throw new KeyNotFoundException($"L'étudiant avec l'ID {etudiantId} n'existe pas.");
        }

        // Suppression de l'étudiant
        await etudiantRepo.DeleteAsync(etudiant);
        await _repositoryFactory.SaveChangesAsync();

        return true;
    }
}