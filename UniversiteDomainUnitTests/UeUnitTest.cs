using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversiteDomainUnitTests
{
    [TestFixture]
    public class UeUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateUeUseCase()
        {
            // Définition des données de test
            string numeroUe = "UE101";
            string intitule = "Programmation avancée";

            // Création d'une UE sans ID (celle qui doit être ajoutée)
            Ue ueSansId = new Ue { NumeroUe = numeroUe, Intitule = intitule };

            // Création du mock de IUeRepository
            var mock = new Mock<IUeRepository>();

            // Simuler que la recherche d'une UE avec le même numéro renvoie une liste vide
            mock.Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()))
                .ReturnsAsync(new List<Ue>());  // Aucune UE existante avec le même numéro

            // Simuler la création d'une UE en base
            Ue ueCree = new Ue { Id = 1, NumeroUe = numeroUe, Intitule = intitule };
            mock.Setup(repo => repo.CreateAsync(It.IsAny<Ue>())).ReturnsAsync(ueCree);

            // Simuler l'appel à SaveChangesAsync()
            mock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Création du repository mocké
            var fauxRepository = mock.Object;

            // Création du Use Case en injectant le faux repository
            CreateUeUseCase useCase = new CreateUeUseCase(fauxRepository);

            // Exécution du Use Case
            var ueTeste = await useCase.ExecuteAsync(ueSansId);

            // Vérifications
            Assert.That(ueTeste.Id, Is.EqualTo(ueCree.Id), "L'ID de l'UE créée doit correspondre.");
            Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueCree.NumeroUe), "Le numéro de l'UE doit être le même.");
            Assert.That(ueTeste.Intitule, Is.EqualTo(ueCree.Intitule), "L'intitulé de l'UE doit être le même.");

            // Vérification que les méthodes du repository ont bien été appelées
            mock.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()), Times.Once);
            mock.Verify(repo => repo.CreateAsync(It.IsAny<Ue>()), Times.Once);
            mock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
