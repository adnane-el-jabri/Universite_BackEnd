using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
//using UniversiteDomain.Entities.SecurityEntities;
//using UniversiteDomain.UseCases.SecurityUseCases;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;
using UniversiteDomain.UseCases.UeUseCases.Create;
using UniversiteDomain.UseCases.NoteUseCases;
using UniversiteDomain.UseCases.SecurityUseCases.Create;


namespace UniversiteDomain.JeuxDeDonnees;

public class BasicBdBuilder(IRepositoryFactory repositoryFactory) : BdBuilder(repositoryFactory)
{
    private readonly string Password = "Miage2025#";

    private readonly Etudiant[] _etudiants =
    [
        new Etudiant { Id=1,NumEtud = "03BDKZ65", Nom = "Dupont", Prenom = "Antoine", Email = "antoine.dupont@etud.u-picardie.fr" },
        new Etudiant { Id=2,NumEtud = "JEIZ03JZ", Nom = "Ntamak", Prenom = "Romain", Email = "roman.ntamak@etud.u-picardie.fr" },
        new Etudiant { Id=3,NumEtud = "62830483", Nom = "Barassi", Prenom = "Pierre-Louis", Email = "pierre-louis.barassi@etud.u-picardie.fr" },
        new Etudiant { Id=4,NumEtud = "J6HZK922", Nom = "Jelong", Prenom = "Antony", Email = "antony.jelong@etud.u-picardie.fr" },
        new Etudiant { Id=5,NumEtud = "PAD89345", Nom = "Akki", Prenom = "Pita", Email = "pita.akki@etud.u-picardie.fr" },
        new Etudiant { Id=6,NumEtud = "RG8647FF", Nom = "Mauvaka", Prenom = "Peato", Email = "peato.mauvaka@etud.u-picardie.fr" }
    ];
    private struct UserNonEtudiant
    {
        public string UserName;
        public string Email;
        public string Role;
    }
    private readonly UserNonEtudiant[] _usersNonEtudiants =
    [
        new UserNonEtudiant { UserName = "anne.lapujade@u-picardie.fr", Email = "anne.lapujade@u-picardie.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "plouisberquez@gmail.com", Email = "plouisberquez@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "jabin.julian.univ@gmail.com", Email = "jabin.julian.univ@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "mehdy.chk@outlook.fr", Email = "mehdy.chk@outlook.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "stephanie.dertin@u-picardie.fr", Email = "stephanie.dertin@u-picardie.fr", Role = "Scolarite" }
    ];

    private readonly Parcours[] _parcours =
    [
        new Parcours { Id=1,NomParcours = "M1", AnneeFormation = 1 },
        new Parcours { Id=2,NomParcours = "OSIE", AnneeFormation = 2 },
        new Parcours { Id=3,NomParcours = "ITD", AnneeFormation = 2 },
        new Parcours { Id=4,NomParcours = "IDD", AnneeFormation = 2 }
    ];

    private readonly Ue[] _ues =
    [
        new Ue { Id=1, NumeroUe = "ISI_01", Intitule = "Architecture des SI 1" },
        new Ue { Id=2, NumeroUe = "ISI_02", Intitule = "Conduite de projet" },
        new Ue { Id=3, NumeroUe = "GEO_05", Intitule = "Marketing" },
        new Ue { Id=4, NumeroUe = "INFO_18", Intitule = "Architecture des SI 2" },
        
    ];

    private struct Inscription
    {
        public long EtudiantId;
        public long ParcoursId;
    }

    private readonly Inscription[] _inscriptions =
    [
        // EtudiantId, ParcoursId
        new Inscription { EtudiantId = 1, ParcoursId = 2 },
        new Inscription { EtudiantId = 2, ParcoursId = 1 },
        new Inscription { EtudiantId = 3, ParcoursId = 1 },
        new Inscription { EtudiantId = 4, ParcoursId = 1 },
        new Inscription { EtudiantId = 5, ParcoursId = 3 },
        new Inscription { EtudiantId = 6, ParcoursId = 4 }
    ];

    private struct UeDansParcours
    {
        public long UeId;
        public long ParcoursId;
    }

    private readonly UeDansParcours[] _maquette =
    [
        new UeDansParcours { UeId = 1, ParcoursId = 1 },
        new UeDansParcours { UeId = 2, ParcoursId = 1 },
        new UeDansParcours { UeId = 3, ParcoursId = 1 },
        new UeDansParcours { UeId = 4, ParcoursId = 2 },
        new UeDansParcours { UeId = 4, ParcoursId = 3 },
        new UeDansParcours { UeId = 4, ParcoursId = 4 },
        
    ];
    
    private struct Note
    {
        public long EtudiantId;
        public long UeId;
        public float Valeur;
    }
    
    private readonly Note[] _notes =
    [
        new Note { UeId = 1, EtudiantId = 2, Valeur = 12 },
        new Note { UeId = 1, EtudiantId = 3, Valeur = (float)8.5 },
        new Note { UeId = 1, EtudiantId = 4, Valeur = 16 },
        new Note { UeId = 2, EtudiantId = 2, Valeur = 14 },
        new Note { UeId = 2, EtudiantId = 3, Valeur = 6 },
        new Note { UeId = 3, EtudiantId = 4, Valeur = (float)11.5 },
        new Note { UeId = 4, EtudiantId = 1, Valeur = 10 },
        new Note { UeId = 4, EtudiantId = 5, Valeur = (float)18.3 },
        new Note { UeId = 4, EtudiantId = 6, Valeur = 12 },
        
        
    ];
    protected override async Task RegenererBdAsync()
    {
        // Ici je décide de supprimer et recréer la BD
        await repositoryFactory.EnsureDeletedAsync();
        await repositoryFactory.EnsureCreatedAsync();
    }
    protected override async Task BuildEtudiantsAsync()
    {
        var etudiantRepo = repositoryFactory.EtudiantRepository();
    
        foreach (Etudiant etudiant in _etudiants)
        {
            var existe = await etudiantRepo.FindByConditionAsync(e => e.Email == etudiant.Email);
            if (existe.Count == 0)
            {
                await new CreateEtudiantUseCase(etudiantRepo).ExecuteAsync(etudiant);
            }
        }
    }


    protected override async Task BuildParcoursAsync()
    {
        var parcoursRepo = repositoryFactory.ParcoursRepository();

        foreach (Parcours parcours in _parcours)
        {
            var parcoursExistant = await parcoursRepo.FindByConditionAsync(p => p.NomParcours == parcours.NomParcours);

            if (parcoursExistant.Count == 0) 
            {
                try
                {
                    await new CreateParcoursUseCase(repositoryFactory).ExecuteAsync(parcours);
                    Console.WriteLine($"✅ Parcours {parcours.NomParcours} créé !");
                
                    // ⚠️ Rafraîchir le contexte pour éviter les erreurs
                    parcoursExistant = await parcoursRepo.FindByConditionAsync(p => p.NomParcours == parcours.NomParcours);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Erreur lors de la création du parcours {parcours.NomParcours} : {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️ Le parcours {parcours.NomParcours} existe déjà.");
            }
        }
    }







    protected override async Task BuildUesAsync()
    {
        var ueRepo = repositoryFactory.UeRepository();
    
        foreach (Ue ue in _ues)
        {
            var existe = await ueRepo.FindByConditionAsync(u => u.NumeroUe == ue.NumeroUe);
            if (existe.Count == 0)
            {
                await new CreateUeUseCase(ueRepo).ExecuteAsync(ue);
            }
        }
    }



    protected override async Task InscrireEtudiantsAsync()
    {
        var etudiantRepo = repositoryFactory.EtudiantRepository();
        var parcoursRepo = repositoryFactory.ParcoursRepository();

        foreach (Inscription i in _inscriptions)
        {
            var etudiant = await etudiantRepo.FindAsync(i.EtudiantId);
            var parcours = await parcoursRepo.FindAsync(i.ParcoursId);

            if (etudiant == null)
            {
                Console.WriteLine($"❌ ERREUR : L'étudiant {i.EtudiantId} n'existe pas !");
                continue; // ⚠️ Passe à l'itération suivante
            }

            if (parcours == null)
            {
                Console.WriteLine($"❌ ERREUR : Le parcours {i.ParcoursId} n'existe pas !");
                continue;
            }

            if (etudiant.ParcoursSuivi == null)
            {
                await new AddEtudiantDansParcoursUseCase(repositoryFactory).ExecuteAsync(i.ParcoursId, i.EtudiantId);
                Console.WriteLine($"✅ Etudiant {etudiant.Nom} inscrit au parcours {parcours.NomParcours}");
            }
            else
            {
                Console.WriteLine($"ℹ️ L'étudiant {etudiant.Nom} est déjà inscrit à un parcours.");
            }
        }
    }





    protected override async Task BuildMaquetteAsync()
    {
        var ueRepo = repositoryFactory.UeRepository();
        var parcoursRepo = repositoryFactory.ParcoursRepository();

        foreach (UeDansParcours u in _maquette)
        {
            var parcours = await parcoursRepo.FindAsync(u.ParcoursId);
            var ue = await ueRepo.FindAsync(u.UeId);

            if (parcours == null)
            {
                Console.WriteLine($"❌ ERREUR : Parcours {u.ParcoursId} introuvable !");
                continue; // ⚠️ Passe à l'itération suivante
            }

            if (ue == null)
            {
                Console.WriteLine($"❌ ERREUR : UE {u.UeId} introuvable !");
                continue;
            }

            if (!parcours.UesEnseignees.Contains(ue))
            {
                await new AddUeDansParcoursUseCase(repositoryFactory).ExecuteAsync(u.ParcoursId, u.UeId);
                Console.WriteLine($"✅ UE {ue.NumeroUe} ajoutée au parcours {parcours.NomParcours}");
            }
        }
    }






    protected override async Task NoterAsync()
    {
        var noteRepo = repositoryFactory.NoteRepository();
        var ueRepo = repositoryFactory.UeRepository();
        var etudiantRepo = repositoryFactory.EtudiantRepository();

        foreach (var note in _notes)
        {
            var existe = await noteRepo.FindByEtudiantAndUeAsync(note.EtudiantId, note.UeId);
            var etudiant = await etudiantRepo.FindAsync(note.EtudiantId);
            var ue = await ueRepo.FindAsync(note.UeId);

            if (existe == null && etudiant != null && ue != null)
            {
                if (etudiant.ParcoursSuivi?.UesEnseignees.Contains(ue) == true)
                {
                    await new AddNoteUseCase(noteRepo, etudiantRepo, ueRepo)
                        .ExecuteAsync(note.EtudiantId, note.UeId, note.Valeur);
                    Console.WriteLine($"✅ Note ajoutée pour {etudiant.Nom} ({note.Valeur}/20) en {ue.Intitule}");
                }
                else
                {
                    Console.WriteLine($"🚨 ERREUR : {etudiant.Nom} n'est pas inscrit à l'UE {ue.Intitule} !");
                }
            }
        }
    }
    protected override async Task BuildRolesAsync()
    {
        // Création des rôles dans la table aspnetroles
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Responsable);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Scolarite);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Etudiant);
    }

    protected override async Task BuildUsersAsync()
    {
        CreateUniversiteUserUseCase uc = new CreateUniversiteUserUseCase(repositoryFactory);
        // Création des étudiants
        foreach (var etudiant in _etudiants)
        {
            await uc.ExecuteAsync(etudiant.Email, etudiant.Email, this.Password, Roles.Etudiant,etudiant);
        }
        
        // Création des responsbles
        foreach (var user in _usersNonEtudiants)
        {
            await uc.ExecuteAsync(user.Email, user.Email, this.Password, user.Role, null);
        }
    }
}