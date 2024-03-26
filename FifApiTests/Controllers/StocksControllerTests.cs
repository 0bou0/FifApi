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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {
        private StocksController _controller;
        private FifaDBContext _context;
        private Mock<FifaDBContext> _mockContext;
        private Mock<IDataRepository<Stock>> _mockRepository;


        public StocksControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FifaDBContext>()
               .UseNpgsql("Server=projet-fifapi.postgres.database.azure.com;Database=postgres;Port=5432;User Id=s212;Password=bQ3i2%C$;Ssl Mode=Require;Trust Server Certificate=true;"); // Chaine de connexion à mettre dans les ( )
            _context = new FifaDBContext(builder.Options);
            _controller = new StocksController(_context);
            _mockRepository = new Mock<IDataRepository<Stock>>();
            _mockContext = new Mock<FifaDBContext>();

        }

        [TestMethod]
        public async Task GetSeriesTest()
        {
            // Act : Appelez la méthode à tester
            var actionResult = await _controller.GetStocks();
            var result = actionResult.Value.ToList();

            // Assert : Vérifiez que le résultat correspond à ce qui est attendu
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetStockbyId_ExistingIdPassed_ReturnsNotFoundResult_AvecMoq()
        {

            // Arrange
            var stock = new Stock
            {
                IdStock = 0,
                TailleId = "S",
                Quantite = 10
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.FindAsync(1)).ReturnsAsync(stock);

            var userController = new StocksController(mockContext.Object);


            
            // Act
            var actionResult = userController.GetStock(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
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

        [TestMethod]
        public async Task PostStock_ValidModel_ReturnsCreatedAtActionResult_AvecMoq()
        {
            // Arrange
            var stock = new Stock
            {
                IdStock = 1,
                TailleId = "M",
                Quantite = 20
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.AddAsync(stock, default));
            mockContext.Setup(c => c.SaveChangesAsync(default));

            var controller = new StocksController(mockContext.Object);

            // Act
            var actionResult = await controller.PostStock(stock);

            // Assert
            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetStock", createdAtActionResult.ActionName);

            var model = createdAtActionResult.Value as Stock;
            Assert.IsNotNull(model);
            Assert.AreEqual(stock.IdStock, model.IdStock);
            Assert.AreEqual(stock.TailleId, model.TailleId);
            Assert.AreEqual(stock.Quantite, model.Quantite);
        }



        /*[TestMethod]
        public async Task PutStock_ValidModel_ReturnsCreatedAtActionResult_AvecMoq()
        {
            // Arrange
            var stock = new Stock
            {
                IdStock = 5,
                TailleId = "M",
                Quantite = 20
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.AddAsync(stock, default));
            mockContext.Setup(c => c.SaveChangesAsync(default));

            var controller = new StocksController(mockContext.Object);

            // Act
            var actionResult = await controller.PutStock(stock.IdStock, stock);

            // Assert
            var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetStock", createdAtActionResult.ActionName);

            var model = createdAtActionResult.Value as Stock;
            Assert.IsNotNull(model);
            Assert.AreEqual(stock.IdStock, model.IdStock);
            Assert.AreEqual(stock.TailleId, model.TailleId);
            Assert.AreEqual(stock.Quantite, model.Quantite);
        }*/







        [TestMethod]
        public async Task DeleteStock_ValidId_ReturnsNoContent_AvecMoq()
        {
            // Arrange : ID du stock à supprimer
            int stockIdToDelete = 10;

            // Ajouter un stock pour le supprimer ensuite
            var stockToDelete = new Stock
            {
                IdStock = stockIdToDelete,
                TailleId = "M",
                Quantite = 10
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.FindAsync(stockIdToDelete)).ReturnsAsync(stockToDelete);
            mockContext.Setup(c => c.Stocks.Remove(stockToDelete));

            var controller = new StocksController(mockContext.Object);

            // Act : Appelez la méthode à tester
            var result = await controller.DeleteStock(stockIdToDelete);

            // Assert : Vérifiez que le stock est supprimé avec succès
            Assert.IsTrue(result is NoContentResult); // En supposant que vous retournez NoContent en cas de suppression réussie

            // Vérifiez si le stock a été réellement supprimé de la base de données
            var deletedStock = await _context.Stocks.FindAsync(stockIdToDelete);
            Assert.IsNull(deletedStock); // Vérifie que le stock est null, ce qui signifie qu'il a été supprimé de la base de données
        }





    }
}
