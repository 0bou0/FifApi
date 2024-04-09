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
        public async Task Get_All_Commandes_Returns_All_Commandes()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                List<Commande> commandes = new List<Commande>(){
                    new Commande { IdCommande = 1, IdUtilisateur = 1, DateCommande = DateTime.Now },
                    new Commande { IdCommande = 2, IdUtilisateur = 2, DateCommande = DateTime.Now }
                };

                dbContext.Commandes.AddRange(commandes);
                dbContext.SaveChanges();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandes();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(commandes.Count, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
            }
        }
        [TestMethod]
        public async Task Get_All_Commandes_Returns_All_Commandes_Equals_To_Zero()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandes();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);

                Assert.AreEqual(0, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
            }
        }


        [TestMethod]
        public async Task Get_Commande_By_Id_Returns_Correct_Item()
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
        public async Task Get_Commande_ById_Returns_Wrong_Item()
        {
            // Arrange
            var idToFind = 100;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Commandes.Add(new Commande { IdCommande = idToFind + 1, IdUtilisateur = 1, DateCommande = DateTime.Now });
                dbContext.Commandes.Add(new Commande { IdCommande = idToFind, IdUtilisateur = 2, DateCommande = DateTime.Now });
                dbContext.SaveChanges();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandeById(idToFind);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreNotEqual(idToFind +1, result.IdCommande);
            }
        }

        [TestMethod]
        public async Task Get_Commande_ById_Returns_Is_False()
        {
            // Arrange
            var idToFind = 100;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Commandes.Add(new Commande { IdCommande = idToFind + 1, IdUtilisateur = 1, DateCommande = DateTime.Now });
                dbContext.SaveChanges();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.GetCommandeById(idToFind);
                var result = actionResult.Value;

                // Assert
                Assert.IsNull(result);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }


        [TestMethod]
        public async Task Post_Commande_Updates_Stock_And_Adds_Commande_With_Lignes()
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

            using (var dbContext = CreateDbContext())
            {

                dbContext.Utilisateurs.Add(new Utilisateur
                {
                    IdUtilisateur = 1,
                    PseudoUtilisateur = "TestUser",
                    MotDePasse = "TestPassword",
                    MailUtilisateur = "test@example.com",
                    Role = "User"
                });

                dbContext.Stocks.AddRange(new List<Stock>
                {
                    new Stock { IdStock = 1, TailleId = "Taille1", Quantite = 10 }, 
                    new Stock { IdStock = 2, TailleId = "Taille2", Quantite = 10 }  
                });

                dbContext.CouleurProduits.AddRange(new List<CouleurProduit>
                {
                    new CouleurProduit { IdCouleurProduit = 1 }, 
                    new CouleurProduit { IdCouleurProduit = 2 } 
                });
                // Act
                await dbContext.SaveChangesAsync();

                var controller = new CommandeController(dbContext);

                
                var actionResult = await controller.PostCommande(newCommande.IdUtilisateur, commandeLine);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

                if (createdAtActionResult.Value != null)
                {
                    var result = createdAtActionResult.Value as Commande;
                    Assert.IsNotNull(result);
                    Assert.AreEqual(newCommande.IdUtilisateur, result.IdUtilisateur);

                    var stocks = await dbContext.Stocks.ToListAsync();
                    Assert.AreEqual(5, stocks.First(s => s.IdStock == 1).Quantite); 
                    Assert.AreEqual(7, stocks.First(s => s.IdStock == 2).Quantite);
                }
                else
                {

                    Assert.Fail("CreatedAtActionResult.Value is null");
                }
            }
        }



        [TestMethod]
        public async Task Post_Commande_Updates_Stock_And_Adds_Commande_With_Lignes_where_stock_null_or_min()
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

            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.Add(new Utilisateur
                {
                    IdUtilisateur = 1,
                    PseudoUtilisateur = "TestUser",
                    MotDePasse = "TestPassword",
                    MailUtilisateur = "test@example.com",
                    Role = "User"
                });

                dbContext.Stocks.AddRange(new List<Stock>
                {
                    new Stock { IdStock = 1, TailleId = "Taille1", Quantite = 1 }, 
                    new Stock { IdStock = 5, TailleId = "Taille2", Quantite = 1 }  
                });

                dbContext.CouleurProduits.AddRange(new List<CouleurProduit>
                {
                    new CouleurProduit { IdCouleurProduit = 1 }, 
                    new CouleurProduit { IdCouleurProduit = 2 } 
                });

                await dbContext.SaveChangesAsync();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.PostCommande(newCommande.IdUtilisateur, commandeLine);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNull(createdAtActionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedObjectResult));


            }
        }

        [TestMethod]
        public async Task Post_Commande_Updates_Stock_And_Adds_Commande_With_Lignes_where_utilisateur_null()
        {
            // Arrange
            var newCommande = new Commande
            {
                IdUtilisateur = 5,

            };
            var commandeLine = new List<CommandLine>
        {
            new CommandLine { IdStock = 1, quantite = 5 ,IdCouleurProduit = 1},
            new CommandLine { IdStock = 2, quantite = 3 , IdCouleurProduit = 2}
        };

            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.Add(new Utilisateur
                {
                    IdUtilisateur = 1,
                    PseudoUtilisateur = "TestUser",
                    MotDePasse = "TestPassword",
                    MailUtilisateur = "test@example.com",
                    Role = "User"
                });

                dbContext.Stocks.AddRange(new List<Stock>
                {
                    new Stock { IdStock = 1, TailleId = "Taille1", Quantite = 1 }, 
                    new Stock { IdStock = 2, TailleId = "Taille2", Quantite = 1 }  
                });

                dbContext.CouleurProduits.AddRange(new List<CouleurProduit>
                {
                    new CouleurProduit { IdCouleurProduit = 1 },
                    new CouleurProduit { IdCouleurProduit = 2 } 
                });

                await dbContext.SaveChangesAsync();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.PostCommande(newCommande.IdUtilisateur, commandeLine);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNull(createdAtActionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedObjectResult));


            }
        }

        [TestMethod]
        public async Task Post_Commande_Updates_Stock_And_Adds_Commande_With_Lignes_where_other_null()
        {
            // Arrange
            var newCommande = new Commande
            {
                IdUtilisateur = 5,

            };
            var commandeLine = new List<CommandLine>
        {
            new CommandLine { IdStock = 1, quantite = 5 ,IdCouleurProduit = 1},
            new CommandLine { IdStock = 2, quantite = 3 , IdCouleurProduit = 2}
        };

            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.Add(new Utilisateur
                {
                    IdUtilisateur = 1,
                    PseudoUtilisateur = "TestUser",
                    MotDePasse = "TestPassword",
                    MailUtilisateur = "test@example.com",
                    Role = "User"
                });

                dbContext.Stocks.AddRange(new List<Stock>
                {
                    new Stock { IdStock = 1, TailleId = "Taille1", Quantite = 1 },

                    new Stock { IdStock = 2, TailleId = "Taille2", Quantite = 1 }  

                });
                dbContext.CouleurProduits.AddRange(new List<CouleurProduit>
                {
                    new CouleurProduit { IdCouleurProduit = 5 }, 
                    new CouleurProduit { IdCouleurProduit = 2 }  
                });

                await dbContext.SaveChangesAsync();

                var controller = new CommandeController(dbContext);

                // Act
                var actionResult = await controller.PostCommande(newCommande.IdUtilisateur, commandeLine);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

                // Assert
                Assert.IsNull(createdAtActionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedObjectResult));


            }
        }


        private FifaDBContext CreateDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FifaDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())); // Utiliser un nom de base de données unique à chaque fois

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }
    }
}
