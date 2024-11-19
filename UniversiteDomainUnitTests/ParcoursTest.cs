using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.ParcoursUseCases.EtudiantDansParcours;

namespace UniversiteDomainUnitTests;

public class ParcoursTest
{
       
    [Test]
    public async Task AddEtudiantDansParcoursUseCase()
    {
        long idEtudiant = 1;
        long idParcours = 3;
        Etudiant etudiant= new Etudiant { Id = 1, NumEtud = "1", Nom = "nom1", Prenom = "prenom1", Email = "1" };
        Parcours parcours = new Parcours{Id=3, NomParcours = "Ue 3", AnneeFormation = 1};
        
        // On initialise une fausse datasource qui va simuler un EtudiantRepository
        var mockEtudiant = new Mock<IEtudiantRepository>();
        var mockParcours = new Mock<IParcoursRepository>();
        List<Etudiant> etudiants = new List<Etudiant>();
        etudiants.Add(new Etudiant{Id=1});
        mockEtudiant
            .Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idEtudiant)))
            .ReturnsAsync(etudiants);

        List<Parcours> parcourses = new List<Parcours>();
        parcourses.Add(parcours);
        
        List<Parcours> parcoursFinaux = new List<Parcours>();
        Parcours parcoursFinal = parcours;
        parcoursFinal.Inscrits.Add(etudiant);
        parcoursFinaux.Add(parcours);
        
        mockParcours
            .Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idParcours)))
            .ReturnsAsync(parcourses);
        mockParcours
            .Setup(repo => repo.AddEtudiantAsync(idParcours, idEtudiant))
            .ReturnsAsync(parcoursFinal);
        // Création du use case en utilisant le mock comme datasource
        AddEtudiantDansParcoursUseCase useCase=new AddEtudiantDansParcoursUseCase(mockEtudiant.Object, mockParcours.Object);
        
        // Appel du use case
        var parcoursTest=await useCase.ExecuteAsync(idParcours, idEtudiant);
        // Vérification du résultat
        Assert.That(parcoursTest.Id, Is.EqualTo(parcoursFinal.Id));
        Assert.That(parcoursTest.Inscrits, Is.Not.Null);
        Assert.That(parcoursTest.Inscrits.Count, Is.EqualTo(1));
        Assert.That(parcoursTest.Inscrits[0].Id, Is.EqualTo(idEtudiant));
    }
}
