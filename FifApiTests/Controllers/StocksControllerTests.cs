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

       




    }
}
