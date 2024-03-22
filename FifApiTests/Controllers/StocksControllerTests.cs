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
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;


namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {
        private StocksController _controller;
        private FifaDBContext _context;

        public StocksControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FifaDBContext>()
                .UseNpgsql("SerieDB"); // Chaine de connexion à mettre dans les ( )
            _context = new FifaDBContext(builder.Options);
            _controller = new StocksController(_context);
        }



        [TestMethod]
        public async Task GetStocks_ExistingIdPassed_ReturnsRightItem()
        {

            // Act : Appelez la méthode à tester
            var actionTask = _controller.GetStocks();
            actionTask.Wait(); // Attend que la tâche soit terminée
            var actionResult = actionTask.Result;

            // Assert : Vérifiez que le résultat correspond à ce qui est attendu

            // Vérifiez si le résultat n'est pas null
            Assert.IsNotNull(actionResult);

            // Vérifiez si le résultat est une instance de ActionResult<IEnumerable<Stock>>
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<IEnumerable<Stock>>));


        }

        [TestMethod]
        public void GetStockbyId_ExistingIdPassed_ReturnsRightItem_AvecMoq()
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
