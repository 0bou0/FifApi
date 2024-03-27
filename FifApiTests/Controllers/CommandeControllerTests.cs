using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FifApi.Migrations;
using FifApi.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using FifApi.Models;

namespace FifApi.Controllers.Tests
{
    [TestClass()]
    public class CommandeControllerTests
    {



        public CommandeControllerTests()
        {


        }




        /*
        [TestMethod]
        public async Task GetAllCommande()
        {
            // Act : Appelez la méthode à tester
            var actionResult = await _controller.GetCommandes();
            var result = actionResult.Value.ToList();

            // Assert : Vérifiez que le résultat correspond à ce qui est attendu
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCommandebyId_ExistingIdPassed_ReturnsNotFoundResult_AvecMoq()
        {

            // Arrange
            var commande = new Commande
            {
                IdCommande = 0,
                IdUtilisateur = 0,
                DateCommande = DateTime.Now
            };




            // Act
            var actionResult = _controller.GetCommandeById(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }


      

        [TestMethod]
        public async Task GetCommandeById_CorrectPassed_ReturnsRightItem()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<FifaDBContext>();
            var controller = new CommandeController(mockContext.Object);

            // Simuler les données de commande attendues
            var expectedCommande = new Commande
            {
                IdCommande = 1,
                IdUtilisateur = 1,
                LigneDeLaCommande = new List<LigneCommande>
        {
            new LigneCommande
            {
                StockLigneCommande = new Stock
                {
                    IdStock = 1,
                    ProduitEncouleur = new CouleurProduit
                    {
                        Couleur_CouleurProduit = new Couleur { Id = 1, Nom = "Rouge" },
                        Produit_CouleurProduit = new Produit { Id = 1, Name = "Produit 1", Description = "Description du Produit 1" },
                    },
                    TailleId = "Small"
                },
                QuantiteAchat = 20
            },
            new LigneCommande
            {
                StockLigneCommande = new Stock
                {
                    IdStock = 2,
                    ProduitEncouleur = new CouleurProduit
                    {
                        Couleur_CouleurProduit = new Couleur { Id = 2, Nom = "Bleu" },
                        Produit_CouleurProduit = new Produit { Id = 1, Name = "Produit 1", Description = "Description du Produit 1" },
                    },
                    TailleId = "Medium"
                },
                QuantiteAchat = 30
            },
            new LigneCommande
            {
                StockLigneCommande = new Stock
                {
                    IdStock = 3,
                    ProduitEncouleur = new CouleurProduit
                    {
                        Couleur_CouleurProduit = new Couleur { Id = 3, Nom = "Vert" },
                        Produit_CouleurProduit = new Produit { Id = 1, Name = "Produit 1", Description = "Description du Produit 1" },
                    },
                    TailleId = "Large"
                },
                QuantiteAchat = 20
            }
        }
            };

            var actionResult = await _controller.GetCommandeById(userId);
            var result = actionResult.Value;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);

            // Vérifier que l'objet retourné correspond à celui attendu
            Assert.AreEqual(expectedCommande.IdCommande, result.IdCommande);
            Assert.AreEqual(expectedCommande.IdUtilisateur, result.IdUtilisateur);
            Assert.AreEqual(expectedCommande.LigneDeLaCommande.Count, result.LigneDeLaCommande.Count);

            for (int i = 0; i < expectedCommande.LigneDeLaCommande.Count; i++)
            {
                var expectedLigne = expectedCommande.LigneDeLaCommande.ToList()[i];
                var resultLigne = result.LigneDeLaCommande.ToList()[i];

                Assert.AreEqual(expectedLigne.StockLigneCommande.IdStock, resultLigne.StockLigneCommande.IdStock);
                Assert.AreEqual(expectedLigne.StockLigneCommande.TailleId, resultLigne.StockLigneCommande.TailleId);
                Assert.AreEqual(expectedLigne.QuantiteAchat, resultLigne.QuantiteAchat);
            }
        }*/





        [TestMethod]
        public async Task GetCommande_ReturnsListOfStocks_avecMoq()
        {
            //Arrange

            var mockContext = new Mock<IDataRepository<Commande>>();


            var stockTable = new CommandeController(mockContext.Object);

            //Act

            stockTable.GetCommandes();

            //Assert

            mockContext.VerifyAll();

        }



    }
}