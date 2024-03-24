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
        private Mock<FifaDBContext> _mockContext;
        private Mock<IDataRepository<Stock>> _mockRepository;


        public StocksControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FifaDBContext>()
                .UseNpgsql("SerieDB"); // Chaine de connexion à mettre dans les ( )
            _context = new FifaDBContext(builder.Options);
            _controller = new StocksController(_context);
            _mockRepository = new Mock<IDataRepository<Stock>>();
            _mockContext = new Mock<FifaDBContext>();

        }



        [TestMethod]
        public async Task GetStocks_ExistingIdPassed_ReturnsRightItem()
        {
            // Act : Appelez la méthode à tester
            var actionTask = _controller.GetStocks();
            actionTask.Wait(); // Attend que la tâche soit terminée
            var actionResult = actionTask.Result;

            // Récupérer les stocks de la base de données
            var stocksFromDb = await _context.Stocks.ToListAsync();

            // Assert : Vérifiez que le résultat correspond à ce qui est attendu

            // Vérifiez si le résultat n'est pas null
            Assert.IsNotNull(actionResult);

            // Vérifiez si le résultat est bien du type ICollection<Stock>
            Assert.IsInstanceOfType(actionResult.Value, typeof(ICollection<Stock>));
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
        [TestMethod]
        public async Task PutStock_ValidIdAndModel_ReturnsNoContent_AvecMoq()
        {
            // Arrange
            var initialStock = new Stock
            {
                IdStock = 20,
                TailleId = "M",
                Quantite = 20
            };

            var updatedStock = new Stock
            {
                IdStock = 20,
                TailleId = "S",
                Quantite = 10
            };

            var mockContext = new Mock<FifaDBContext>();
            mockContext.Setup(c => c.Stocks.AddAsync(initialStock, default));
            mockContext.Setup(c => c.SaveChangesAsync(default));

            var controller = new StocksController(mockContext.Object);

            // Act
            var actionResult = await controller.PostStock(initialStock);

            mockContext.Setup(c => c.Stocks.Update(updatedStock));
            mockContext.Setup(c => c.SaveChangesAsync(default));

            // Act
            var actionNewResult = await controller.PutStock(initialStock.IdStock, updatedStock);

            // Assert
            var createdAtActionResult = await controller.GetStock(initialStock.IdStock);
            Assert.IsNotNull(createdAtActionResult);

            // Vérifiez si l'action a renvoyé un objet OkObjectResult
            if (createdAtActionResult.Result is OkObjectResult okObjectResult)
            {
                var model = (Stock)okObjectResult.Value;
                Assert.IsNotNull(model);
                Assert.AreEqual(updatedStock.IdStock, model.IdStock);
                Assert.AreEqual(updatedStock.TailleId, model.TailleId);
                Assert.AreEqual(updatedStock.Quantite, model.Quantite);
            }
            else
            {
                Assert.Fail("La méthode GetStock n'a pas renvoyé un OkObjectResult.");
            }
        }


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
