using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NoteExceptions;
using UniversiteDomain.UseCases.NoteUseCases;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversiteDomainUnitTests;

[TestFixture]
public class NoteUnitTest
{
    private Mock<INoteRepository> mockNoteRepo;
    private Mock<IEtudiantRepository> mockEtudiantRepo;
    private Mock<IUeRepository> mockUeRepo;

    [SetUp]
    public void Setup()
    {
        // Initialisation des mocks
        mockNoteRepo = new Mock<INoteRepository>();
        mockEtudiantRepo = new Mock<IEtudiantRepository>();
        mockUeRepo = new Mock<IUeRepository>();
    }

    [Test]
    public async Task AddNote_Should_Add_Note_To_Student()
    {
        try
        {
            // Données d'entrée
            long etudiantId = 1;
            long ueId = 2;
            float noteValeur = 15.5f;

            // Création des objets simulés
            var ue = new Ue { Id = ueId, NumeroUe = "UE1", Intitule = "Mathématiques" };
            var parcours = new Parcours { Id = 10, NomParcours = "Licence Informatique", UesEnseignees = new List<Ue> { ue } };
            var etudiant = new Etudiant { Id = etudiantId, NumEtud = "ETU123", Nom = "Doe", Prenom = "John", Email = "john.doe@example.com", ParcoursSuivi = parcours };

            // Configuration des mocks
            mockEtudiantRepo.Setup(repo => repo.FindAsync(etudiantId)).ReturnsAsync(etudiant);
            mockUeRepo.Setup(repo => repo.FindAsync(ueId)).ReturnsAsync(ue);
            mockNoteRepo.Setup(repo => repo.FindByEtudiantAndUeAsync(etudiantId, ueId)).ReturnsAsync((Note)null);
            mockNoteRepo.Setup(repo => repo.CreateAsync(It.IsAny<Note>())).ReturnsAsync((Note note) => note);
            mockNoteRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Instanciation du use case
            var useCase = new AddNoteUseCase(mockNoteRepo.Object, mockEtudiantRepo.Object, mockUeRepo.Object);

            // Exécution du use case
            var result = await useCase.ExecuteAsync(etudiantId, ueId, noteValeur);

            // Vérifications des assertions
            Assert.NotNull(result);
            Assert.That(result.Valeur, Is.EqualTo(noteValeur));
            Assert.That(result.Etudiant.Id, Is.EqualTo(etudiantId));
            Assert.That(result.Ue.Id, Is.EqualTo(ueId));

        }
        catch (Exception ex)
        {
            Assert.Fail($"Test échoué avec exception : {ex.Message}\nStackTrace : {ex.StackTrace}");
        }
    }
}
