using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using FifApi.Models;
using FifApi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class CommandeControllerTests
    {
        [TestMethod]
        public async Task GetAllCommandes_ReturnsAllCommandes()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Commandes.AddRange(new[]
                {
                    new Commande { IdCommande = 1, IdUtilisateur = 1, DateCommande = DateTime.Now },
                    new Commande { IdCommande = 2, IdUtilisateur = 2, DateCommande = DateTime.Now }
                });
                dbContext.SaveChanges();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandes();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
            }
        }

        [TestMethod]
        public async Task GetCommandeById_ReturnsCorrectItem()
        {
            // Arrange
            var idToFind = 100;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Commandes.Add(new Commande { IdCommande = idToFind, IdUtilisateur = 1, DateCommande = DateTime.Now });
                dbContext.SaveChanges();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandeById(idToFind);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(idToFind, result.IdCommande);
            }
        }

        [TestMethod]
        public async Task PostCommande_UpdatesStockAndAddsCommandeWithLignes()
        {
            // Arrange
            var newCommande = new Commande
            {
                IdUtilisateur = 1,

            };
            var commandeLine = new List<CommandLine>
        {
            new CommandLine { IdStock = 1, quantite = 5 ,IdCouleurProduit = 1},
            new CommandLine { IdStock = 2, quantite = 3 , IdCouleurProduit = 2}
        };

            // Créer un contexte de base de données en mémoire pour les tests
            using (var dbContext = CreateDbContext())
            {
                // Ajouter un utilisateur de test avec des valeurs requises
                dbContext.Utilisateurs.Add(new Utilisateur
                {
                    IdUtilisateur = 1,
                    PseudoUtilisateur = "TestUser",
                    MotDePasse = "TestPassword",
                    MailUtilisateur = "test@example.com",
                    Role = "User"
                });

                // Ajouter des stocks de test avec des valeurs requises
                dbContext.Stocks.AddRange(new List<Stock>
                {
                    new Stock { IdStock = 1, TailleId = "Taille1", Quantite = 10 }, // IdStock = 1
                    new Stock { IdStock = 2, TailleId = "Taille2", Quantite = 10 }  // IdStock = 2
                });

                        // Ajouter des couleursproduit de test
                        dbContext.CouleurProduits.AddRange(new List<CouleurProduit>
                {
                    new CouleurProduit { IdCouleurProduit = 1 }, // IdCouleurProduit = 1
                    new CouleurProduit { IdCouleurProduit = 2 }  // IdCouleurProduit = 2
                });

                await dbContext.SaveChangesAsync();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.PostCommande(newCommande.IdUtilisateur, commandeLine);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

                // Vérifier si createdAtActionResult.Value est null avant de l'utiliser
                if (createdAtActionResult.Value != null)
                {
                    var result = createdAtActionResult.Value as Commande;
                    Assert.IsNotNull(result);
                    Assert.AreEqual(newCommande.IdUtilisateur, result.IdUtilisateur);

                    // Vérifier si la quantité du stock a été mise à jour correctement
                    var stocks = await dbContext.Stocks.ToListAsync();
                    Assert.AreEqual(5, stocks.First(s => s.IdStock == 1).Quantite); // Vérifier le stock avec IdStock = 1
                    Assert.AreEqual(7, stocks.First(s => s.IdStock == 2).Quantite); // Vérifier le stock avec IdStock = 2
                }
                else
                {
                    // Gérer le cas où createdAtActionResult.Value est null
                    Assert.Fail("CreatedAtActionResult.Value is null");
                }
            }
        }

        private FifaDBContext CreateDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FifaDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: "TestCommandeDatabase"));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }

    }
}
