using UniversiteDomain.DataAdapters;

namespace UniversiteDomain.JeuxDeDonnees;

public abstract class BdBuilder(IRepositoryFactory repositoryFactory)
{
    public async Task BuildUniversiteBdAsync()
    {
        // Suppression de la BD
        Console.WriteLine("Suppression et recréation de la BD");
        await RegenererBdAsync();
        
        Console.WriteLine("BuildParcours");
        await BuildParcoursAsync();
        Console.WriteLine("BuidEtudiants");
        await BuildEtudiantsAsync();
        Console.WriteLine("BuildUes");
        await BuildUesAsync();
        Console.WriteLine("BuildMaquette");
        await BuildMaquetteAsync();
        Console.WriteLine("InscrireEtudiants");
        await InscrireEtudiantsAsync();
        Console.WriteLine("Noter");
        await NoterAsync();
        
        // Gestion de la sécurité
        /* A décommenter quand on aura rajouté la sécu
// Création des rôles
Console.WriteLine("BuildRoles");
//await BuildRolesAsync();
// Création des utilisateurs
Console.WriteLine("BuildUsers");
//wait BuildUsersAsync();
        */
    }

    protected abstract Task RegenererBdAsync();
    protected abstract Task BuildRolesAsync();
    protected abstract Task BuildUsersAsync();
    protected abstract Task BuildParcoursAsync();
    protected abstract Task BuildEtudiantsAsync();
    protected abstract Task BuildUesAsync();
    protected abstract Task InscrireEtudiantsAsync();
    protected abstract Task BuildMaquetteAsync();
    protected abstract Task NoterAsync();
}