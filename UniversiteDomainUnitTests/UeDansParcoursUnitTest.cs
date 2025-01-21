using Moq;
using System.Linq.Expressions;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.ParcoursUseCases.UeDansParcours;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversiteDomainUnitTests
{
    [TestFixture]
    public class UeDansParcoursUnitTest
    {
        private Mock<IUeRepository> mockUeRepo;
        private Mock<IParcoursRepository> mockParcoursRepo;
        private Mock<IRepositoryFactory> mockFactory;

        [SetUp]
        public void Setup()
        {
            mockUeRepo = new Mock<IUeRepository>();
            mockParcoursRepo = new Mock<IParcoursRepository>();
            mockFactory = new Mock<IRepositoryFactory>();

            mockFactory.Setup(f => f.UeRepository()).Returns(mockUeRepo.Object);
            mockFactory.Setup(f => f.ParcoursRepository()).Returns(mockParcoursRepo.Object);
        }

        [Test]
        public async Task AddUeDansParcoursUseCase_Should_Add_Ue_To_Parcours()
        {
            // Définition des IDs
            long idUe = 1;
            long idParcours = 3;

            // Création d'un objet UE
            Ue ue = new Ue { Id = idUe, NumeroUe = "UE101", Intitule = "Programmation avancée" };
            
            // Création d'un objet Parcours
            Parcours parcours = new Parcours { Id = idParcours, NomParcours = "Informatique", AnneeFormation = 1 };

            // Simulation de la récupération de l'UE
            mockUeRepo
                .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()))
                .ReturnsAsync(new List<Ue> { ue });

            // Simulation de la récupération du parcours
            mockParcoursRepo
                .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Parcours, bool>>>()))
                .ReturnsAsync(new List<Parcours> { parcours });

            // Simulation de l'ajout de l'UE dans le parcours
            mockParcoursRepo
                .Setup(repo => repo.AddUeAsync(idParcours, idUe))
                .ReturnsAsync(parcours);

            // Création du Use Case
            AddUeDansParcoursUseCase useCase = new AddUeDansParcoursUseCase(mockFactory.Object);

            // Exécution du Use Case
            var parcoursTest = await useCase.ExecuteAsync(idParcours, idUe);

            // Vérification des résultats
            Assert.That(parcoursTest.Id, Is.EqualTo(parcours.Id));
            Assert.That(parcoursTest.NomParcours, Is.EqualTo(parcours.NomParcours));
            Assert.That(parcoursTest.AnneeFormation, Is.EqualTo(parcours.AnneeFormation));
        }
    }
}
