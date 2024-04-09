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
using NuGet.Protocol;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class StocksControllerTests
    {
        [TestMethod]
        public async Task Get_All_Stocks()
        {
            List<Stock> stocks = new List<Stock>() {       new Stock { IdStock = 1, TailleId = "l", Quantite = 10 },
                    new Stock { IdStock = 2, TailleId = "s", Quantite = 20 }};

            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.AddRange(stocks);
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.GetStocks();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(stocks.Count, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
            }
        }

        [TestMethod]
        public async Task Get_All_Stocks_Returns_Null()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.GetStocks();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
                Assert.IsNotNull(actionResult);
                Assert.IsNotNull(actionResult.Value);
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
                Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
                Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);

            }
        }


        [TestMethod]
        public async Task Get_Stock_By_Id_Returns_Correct_Item()
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
        public async Task Get_Stock_By_Id_Returns_Wrong_Item()
        {
            // Arrange
            var idToFind = 100;
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(new Stock { IdStock = idToFind + 1, TailleId = "l", Quantite = 10 });
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.GetStock(idToFind);
                var result = actionResult.Value;
                // Assert
                Assert.IsNull(result);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }


        [TestMethod]
        public async Task Post_Stock_Returns_Created_Response()
        {
            // Arrange
            var newStock = new Stock { IdStock = 3, TailleId = "M", Quantite = 30 };
            var newTaille = new Taille { IdTaille = "M", DescriptionTaille = "tres large", NomTaille = "M" };
            using (var dbContext = CreateDbContext())
            {
                dbContext.Tailles.Add(newTaille);

                var controller = new StocksController(dbContext);
                // Act
                var actionResult = await controller.PostStock(newStock);
                Assert.Fail(actionResult.ToJson());
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
        public async Task Post_Stock_Returns_NoContent_When_Quantity_Is_NonPositive()
        {
            // Arrange
            var newStock = new Stock { IdStock = 3, TailleId = "M", Quantite = -30 };
            using (var dbContext = CreateDbContext())
            {
                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.PostStock(newStock);

                // Assert
                Assert.IsInstanceOfType(actionResult.Result, typeof(NoContentResult));
            }
        }






        [TestMethod]
        public async Task Put_Stock_Returns_NoContent_Response()
        {
            // Arrange
            var idToUpdate = 50;
            var updatedNewStock = new Stock { IdStock = idToUpdate, TailleId = "l" };
            var updatedStock = new Stock { IdStock = idToUpdate, TailleId = "l", Quantite = 50 };
            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(updatedStock);
                dbContext.SaveChanges();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.PutStock(idToUpdate, updatedNewStock);
                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            }
        }


        [TestMethod]
        public async Task Put_Stock_Returns_NoContent_When_Quantity_Is_NonPositive()
        {
            // Arrange
            var idToUpdate = 5;
            var stockToUpdate = new Stock { IdStock = idToUpdate, TailleId = "M", Quantite = 0 };

            using (var dbContext = CreateDbContext())
            {
                dbContext.Stocks.Add(stockToUpdate);
                await dbContext.SaveChangesAsync();

                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.PutStock(idToUpdate, stockToUpdate);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            }
        }

    


        [TestMethod]
        public async Task Delete_Stock_Returns_NoContent_Response()
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


        [TestMethod]
        public async Task Delete_Stock_Returns_NoFound_Response()
        {
            // Arrange
            var idToDelete = 100; 

            using (var dbContext = CreateDbContext())
            {
                var controller = new StocksController(dbContext);

                // Act
                var actionResult = await controller.DeleteStock(idToDelete);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            }
        }

        private FifaDBContext CreateDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FifaDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())); 

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<FifaDBContext>();
        }

    }
}
