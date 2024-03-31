using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using FifApi.Models;
using FifApi.Models.EntityFramework;
using System.Linq;
using System.Threading.Tasks;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {
        [TestMethod]
        public async Task GetAllStocks()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.AddRange(new[]
                {
                    new Stock { IdStock = 1, TailleId = "l", Quantite = 10 },
                    new Stock { IdStock = 2, TailleId = "s", Quantite = 20 }
                });
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.GetStocks();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
            }
        }

        [TestMethod]
        public async Task GetStockById_ReturnsCorrectItem()
        {
            // Arrange
            var idToFind = 100;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(new Stock { IdStock = idToFind, TailleId = "l", Quantite = 10 });
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.GetStock(idToFind);
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(idToFind, result.IdStock);
            }
        }

        [TestMethod]
        public async Task PostStock_ReturnsCreatedResponse()
        {
            // Arrange
            var newStock = new Stock { IdStock = 3, TailleId = "m", Quantite = 30 };
            using (var dbContext = CreateDbContext())
            {
                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.PostStock(newStock);
                var createdAtActionResult = actionResult.Result as CreatedAtActionResult;
                var result = createdAtActionResult.Value as Stock;

                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
                Assert.IsNotNull(result);
                Assert.AreEqual(newStock.IdStock, result.IdStock);
            }
        }

        [TestMethod]
        public async Task PutStock_ReturnsNoContentResponse()
        {
            // Arrange
            var idToUpdate = 50;
            var updatedStock = new Stock { IdStock = idToUpdate, TailleId = "l", Quantite = 50 };
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(updatedStock);
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.PutStock(idToUpdate, updatedStock);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            }
        }

        [TestMethod]
        public async Task DeleteStock_ReturnsNoContentResponse()
        {
            // Arrange
            var idToDelete = 2;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(new Stock { IdStock = idToDelete, TailleId = "s", Quantite = 20 });
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.DeleteStock(idToDelete);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            }
        }

        private FifaDBContext CreateDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FifaDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: "TestStockDatabase"));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }
    }
}
