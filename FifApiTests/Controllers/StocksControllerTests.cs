using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FifApi.Controllers;
using FifApi.Models.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FifApi.Models;
using System.Collections;
using System.Linq.Expressions;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {
        [TestMethod]
        public void GetStock_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            var stock = new Stock
            {
                IdStock = 1,
                TailleId = "S",
                Quantite = 10
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.FindAsync(1)).ReturnsAsync(stock);

            var userController = new StocksController(mockContext.Object);


            // Act
            var actionResult = userController.GetStock(1).Result;
            var result = actionResult.Value;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);

            // Asserts sur les propriétés du stock retourné
            Assert.AreEqual(stock.IdStock, result.IdStock);
            Assert.AreEqual(stock.TailleId, result.TailleId);
            Assert.AreEqual(stock.Quantite, result.Quantite);

            // Autres assertions si nécessaire
            Assert.IsTrue(result.Quantite > 0); // Vérifie si la quantité est positive
            Assert.IsFalse(string.IsNullOrEmpty(result.TailleId)); // Vérifie si la taille n'est pas nulle ou vide

        }

        [TestMethod]
        public void GetStocksAll_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            var stocks = new List<Stock>
            {
                new Stock { IdStock = 1, TailleId = "S", Quantite = 10 },
                new Stock { IdStock = 2, TailleId = "M", Quantite = 20 }
                // Ajoutez d'autres stocks au besoin
            };

            var mockRepository = new Mock<IDataRepository<Stock>>();
            mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(stocks);


            var userController = new StocksController(mockRepository.Object);

            // Act
            var actionResult = userController.GetStocks().Result;
            var result = actionResult.Value.ToList(); // Convertir IEnumerable en List pour faciliter l'assertion

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(result);
            Assert.AreEqual(stocks.Count, result.Count); // Vérifie si le nombre d'éléments retournés est le même que celui de la liste originale

            // Vérifie si les propriétés des stocks retournés correspondent aux propriétés des stocks originaux
            for (int i = 0; i < stocks.Count; i++)
            {
                Assert.AreEqual(stocks[i].IdStock, result[i].IdStock);
                Assert.AreEqual(stocks[i].TailleId, result[i].TailleId);
                Assert.AreEqual(stocks[i].Quantite, result[i].Quantite);
            }
        }





    }
}
