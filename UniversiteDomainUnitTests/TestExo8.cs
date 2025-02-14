using Xunit;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases;
using UniversiteDomain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Assert = Xunit.Assert;

public class GenerateCsvForUeNotesUseCaseTests
{
    private readonly Mock<IEtudiantRepository> _mockEtudiantRepo;
    private readonly Mock<IUeRepository> _mockUeRepo;
    private readonly GenerateCsvForUeNotesUseCase _useCase;

    public GenerateCsvForUeNotesUseCaseTests()
    {
        _mockEtudiantRepo = new Mock<IEtudiantRepository>();
        _mockUeRepo = new Mock<IUeRepository>();
        _useCase = new GenerateCsvForUeNotesUseCase(_mockEtudiantRepo.Object, _mockUeRepo.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldGenerateCsv_WhenDataIsValid()
    {
        // Arrange
        var ue = new Ue { Id = 1, Intitule = "Mathematiques" };
        var etudiants = new List<Etudiant>
        {
            new Etudiant { Id = 1, Nom = "Dupont", Prenom = "Jean" },
            new Etudiant { Id = 2, Nom = "Durand", Prenom = "Marie" }
        };

        _mockUeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ue);
        _mockEtudiantRepo.Setup(repo => repo.GetEtudiantsByUeIdAsync(1)).ReturnsAsync(etudiants);

        // Act
        var result = await _useCase.ExecuteAsync(1);

        // Assert
        Assert.Contains("NumEtud,Nom,Prenom,Note", result);
        Assert.Contains("1,Dupont,Jean", result);
        Assert.Contains("2,Durand,Marie", result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowCsvProcessingException_WhenUeNotFound()
    {
        // Arrange
        _mockUeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Ue)null);

        // Act & Assert
        await Assert.ThrowsAsync<CsvProcessingException>(() => _useCase.ExecuteAsync(1));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowCsvProcessingException_WhenNoStudentsFound()
    {
        // Arrange
        var ue = new Ue { Id = 1, Intitule = "Mathematiques" };
        _mockUeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ue);
        _mockEtudiantRepo.Setup(repo => repo.GetEtudiantsByUeIdAsync(1)).ReturnsAsync(new List<Etudiant>());

        // Act & Assert
        await Assert.ThrowsAsync<CsvProcessingException>(() => _useCase.ExecuteAsync(1));
    }
}

public class UploadCsvForUeNotesUseCaseTests
{
    private readonly Mock<INoteRepository> _mockNoteRepo;
    private readonly Mock<IUeRepository> _mockUeRepo;
    private readonly UploadCsvForUeNotesUseCase _useCase;
    private readonly Mock<IEtudiantRepository> _mockEtudiantRepo;

    public UploadCsvForUeNotesUseCaseTests()
    {
        _mockNoteRepo = new Mock<INoteRepository>();
        _mockUeRepo = new Mock<IUeRepository>();
        _mockEtudiantRepo = new Mock<IEtudiantRepository>();
        var validationUseCase = new ValidationUseCase(_mockEtudiantRepo.Object, _mockUeRepo.Object);
        _useCase = new UploadCsvForUeNotesUseCase(_mockNoteRepo.Object, validationUseCase);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessCsvCorrectly_WhenDataIsValid()
    {
        // Arrange
        var ue = new Ue { Id = 1, Intitule = "Mathematiques" };
        var etudiants = new List<Etudiant>
        {
            new Etudiant { Id = 1, Nom = "Dupont", Prenom = "Jean" },
            new Etudiant { Id = 2, Nom = "Durand", Prenom = "Marie" }
        };

        _mockUeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ue);

        // Simulation pour que les étudiants soient trouvés dans le repository
        _mockEtudiantRepo.Setup(repo => repo.GetEtudiantsByUeIdAsync(1)).ReturnsAsync(new List<Etudiant> { etudiants[0] });
        _mockEtudiantRepo.Setup(repo => repo.GetEtudiantsByUeIdAsync(2)).ReturnsAsync(new List<Etudiant> { etudiants[1] });

        var csvContent = "NumEtud,Nom,Prenom,Note\n1,Dupont,Jean,15\n2,Durand,Marie,18";
        using var csvStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));

        // Act
        await _useCase.ExecuteAsync(csvStream, 1);

        // Assert
        _mockNoteRepo.Verify(repo => repo.SaveOrUpdateAsync(It.IsAny<Note>()), Times.Exactly(2));
    }


    [Fact]
    public async Task ExecuteAsync_ShouldThrowNoteFormatException_WhenNoteIsInvalid()
    {
        // Arrange
        var ue = new Ue { Id = 1, Intitule = "Mathematiques" };
        _mockUeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ue);

        var csvContent = "NumEtud,Nom,Prenom,Note\n1,Dupont,Jean,25"; // Note invalide
        using var csvStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));

        // Act & Assert
        await Assert.ThrowsAsync<CsvProcessingException>(() => _useCase.ExecuteAsync(csvStream, 1));
    }
}
