using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Entities;

namespace UniversiteEFDataProvider.Data;
 
public class UniversiteDbContext : IdentityDbContext<UniversiteUser>
{
    public static readonly ILoggerFactory consoleLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    public UniversiteDbContext(DbContextOptions<UniversiteDbContext> options)
        : base(options)
    {
    }
 
    public UniversiteDbContext():base()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(consoleLogger)  //on lie le contexte avec le système de journalisation
            .EnableSensitiveDataLogging() 
            .EnableDetailedErrors();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Propriétés de la table Etudiant
        // Clé primaire
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Etudiant>()
            .HasKey(e => e.Id);
        // ManyToOne vers parcours
        modelBuilder.Entity<Etudiant>()
            .HasOne(e => e.ParcoursSuivi)
            .WithMany(p => p.Inscrits);
        // OneToMany vers Note
        modelBuilder.Entity<Etudiant>()
            .HasMany(e => e.Notes)
            .WithOne(n => n.Etudiant);
        
        // Propriétés de la table Parcours
        // Clé primaire
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Parcours>()
            .HasKey(p => p.Id);
        // ManyToOne vers Etudiant
        modelBuilder.Entity<Parcours>()
            .HasMany(p => p.Inscrits)
            .WithOne(e => e.ParcoursSuivi);
        // ManyToMany vers Ue
        modelBuilder.Entity<Parcours>()
            .HasMany(p => p.UesEnseignees)
            .WithMany(ue => ue.EnseigneeDans);

        // Propriétés de la table Ue
        // Clé primaire
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ue>()
            .HasKey(ue => ue.Id);
        // ManyToMany vers Parcours
        modelBuilder.Entity<Ue>()
            .HasMany(ue => ue.EnseigneeDans)
            .WithMany(p => p.UesEnseignees);
        // OneToMany vers Note
        modelBuilder.Entity<Ue>()
            .HasMany(ue => ue.Notes)
            .WithOne(n => n.Ue);
        
        // Propriétés de la table Note
        // Clé primaire composite
        modelBuilder.Entity<Note>()
            .HasKey(n => new { n.EtudiantId, n.UeId });
        // ManyToOne vers Etudiant
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Etudiant)
            .WithMany(e => e.Notes);
        // ManyToOne vers Ue
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Ue)
            .WithMany(ue => ue.Notes);
        // Propriétés de la table UniversiteUser
        //OneToOne vers UniversityUser
        modelBuilder.Entity<UniversiteUser>()
            .HasOne<Etudiant>(user => user.Etudiant)
            .WithOne()
            .HasForeignKey<Etudiant>();
        modelBuilder.Entity<Etudiant>()
            .HasOne<UniversiteUser>()
            .WithOne(user => user.Etudiant)
            .HasForeignKey<UniversiteUser>(user => user.EtudiantId);
        // Permet d'inclure automatiquement l'étudiant dans le user sans avoir besoin de préciser la jointure
        modelBuilder.Entity<UniversiteUser>().Navigation<Etudiant>(user => user.Etudiant).AutoInclude();
        modelBuilder.Entity<UniversiteRole>();
    }
    public DbSet <Parcours>? Parcours { get; set; }
    public DbSet <Etudiant>? Etudiants { get; set; }
    public DbSet <Ue>? Ues { get; set; }
    public DbSet <Note>? Notes { get; set; }
    public DbSet <UniversiteUser>? UniversiteUsers { get; set; }
    public DbSet<UniversiteRole>? UniversiteRoles { get; set; }
}