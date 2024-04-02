using FifApi.Controllers;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTests
    {
        [TestMethod]
        public async Task Get_Nations_Returns_List_Of_Nations()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Pays.AddRange(new[]
                {
                    new Pays { IdPays = "fr", NomPays = "France" },
                    new Pays { IdPays = "esp", NomPays = "Espagne" }
                });
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetNations();
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
        public async Task Get_Nations_Returns_List_Of_Nations_Equal_To_Zero()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetNations();
                var result = actionResult.Value; 

                // Assert
                Assert.IsNull(result); 
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult)); 
            }
        }


        [TestMethod]
        public async Task Get_Categories_Returns_List_Of_Categories()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.TypeProduits.AddRange(new[]
                {
                    new TypeProduit { Id = 1, Nom = "Catégorie 1" },
                    new TypeProduit { Id = 2, Nom = "Catégorie 2" }
                });
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetCategories();
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
        public async Task Get_Categories_Returns_List_Of_Categories_Equal_To_Zero()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
               
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetCategories();
                var result = actionResult.Value;

                // Assert
                Assert.IsNull(result);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task Get_Couleurs_Returns_List_Of_Couleurs()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Couleurs.AddRange(new[]
                {
                    new Couleur { Id = 1, Nom = "Rouge", Hexa = "#FF0000" },
                    new Couleur { Id = 2, Nom = "Vert", Hexa = "#00FF00" }
                });
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetCouleurs();
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
        public async Task Get_Couleurs_Returns_List_Of_Couleurs_Equals_To_Zero()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetCouleurs();
                var result = actionResult.Value;

                // Assert
                Assert.IsNull(result);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task Get_Tailles_Returns_List_Of_Tailles_Equal_To_Zero()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetTailles();
                var result = actionResult.Value;

                // Assert
                Assert.IsNull(result);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task Get_Tailles_Returns_List_Of_Tailles()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                dbContext.Tailles.AddRange(new[]
                {
                    new Taille { IdTaille = "S", NomTaille = "Small" },
                    new Taille { IdTaille = "M", NomTaille = "Medium" }
                });
                dbContext.SaveChanges();

                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.GetTailles();
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