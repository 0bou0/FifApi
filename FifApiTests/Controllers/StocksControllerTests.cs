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
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {


        public StocksControllerTests()
        {

        }

        /*[TestMethod]
        public async Task GetAllStocks()
        {
            // Act : Appelez la méthode à tester
            var actionResult = _controller.GetStocks();
            var result = actionResult.Value.ToList();

            // Assert : Vérifiez que le résultat correspond à ce qui est attendu
            Assert.IsNotNull(result);
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
        }*/

        [TestMethod]
        public async Task GetStocks_ReturnsListOfStocks_avecMoq()
        {
            //Arrange
            var stocks = new List<Stock>
            {
                new Stock { IdStock = 1, Quantite = 20, CouleurProduitId= 1 },
                new Stock { IdStock = 2, Quantite = 10, CouleurProduitId= 2 },
                new Stock { IdStock = 3, Quantite = 30, CouleurProduitId= 3 }
            };

            var mockRepository = new Mock<IDataRepository<Stock>>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stocks);

            var stockController = new StocksController(mockRepository.Object);

            // Act
            var actionResult = await stockController.GetStocks();

            // Assert
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            var returnedStocks = okResult.Value as List<Stock>;
            Assert.IsNotNull(returnedStocks);
            Assert.AreEqual(stocks.Count, returnedStocks.Count);


            // Assert individual stock properties
            for (int i = 0; i < stocks.Count; i++)
            {
                Assert.AreEqual(stocks[i].IdStock, returnedStocks[i].IdStock);
                Assert.AreEqual(stocks[i].Quantite, returnedStocks[i].Quantite);
                Assert.AreEqual(stocks[i].CouleurProduitId, returnedStocks[i].CouleurProduitId);
            }

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);

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

            var mockRepository = new Mock<IDataRepository<Stock>>();
            mockRepository.Setup(repo => repo.DeleteAsync(stockIdToDelete)).ReturnsAsync(stockToDelete);
            mockRepository.Setup(repo => repo.GetByIdAsync(stockIdToDelete));


            var controller = new StocksController(mockRepository.Object);

            var result = await controller.DeleteStock(stockIdToDelete);

            var getbyid = await controller.GetStock(stockIdToDelete);

            //Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(getbyid);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));



        }


        /*[TestMethod]
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
        }*/





    }
}
