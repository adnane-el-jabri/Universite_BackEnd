﻿using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface IRepositoryFactory
{
    IParcoursRepository ParcoursRepository();
    IEtudiantRepository EtudiantRepository();
    IUeRepository UeRepository();
    INoteRepository NoteRepository();
    IUniversiteRoleRepository UniversiteRoleRepository();
    IUniversiteUserRepository UniversiteUserRepository();
    
    
    
    
    
    // Gestion de la base de données
    Task EnsureDeletedAsync();
    Task EnsureCreatedAsync();
    Task SaveChangesAsync();
    Task<Etudiant> CreateAsync(Etudiant etudiant);
    
}