using FifApi.Controllers;
using FifApi.Models.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Protocol;
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
        public async Task Post_Nations_Returns_Created_Response()
        {
            // Arrange
            var newPays = new Pays { IdPays = "FR", NomPays = "France" };
            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.PostNations(newPays);
                var createdAtActionResult = actionResult as OkResult;
                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status200OK, createdAtActionResult.StatusCode);

                // Check if the country is actually added to the database
                var paysInDatabase = dbContext.Pays.FirstOrDefault(p => p.NomPays == newPays.NomPays);
                Assert.IsNotNull(paysInDatabase);
                Assert.AreEqual(newPays.IdPays, paysInDatabase.IdPays);
            }
        }

        [TestMethod]
        public async Task Post_Nations_Returns_Bad_Request_If_Nation_Already_Exists()
        {
            // Arrange
            var pays = new Pays { IdPays = "GER", NomPays = "Germany" };
            var existingPays = new Pays { IdPays = "GER", NomPays = "Germany" };


            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                await controller.PostNations(pays);
                var actionResult = await controller.PostNations(existingPays);


                // Assert
                var badRequestResult = actionResult as BadRequestObjectResult;
                Assert.IsNotNull(badRequestResult);
                Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.AreEqual("nation already in base", badRequestResult.Value);
            }
        }


        [TestMethod]
        public async Task Put_Nations_Returns_Created_Response()
        {
            // Arrange
            var pays = new Pays { IdPays = "GER", NomPays = "Germany" };
            var newPays = new Pays { IdPays = "GER", NomPays = "GERMANY" };


            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                await controller.PostNations(pays);
                var actionResult = await controller.PutNations(newPays);


                // Assert
                var createdAtActionResult = actionResult as OkResult;
                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status200OK, createdAtActionResult.StatusCode);

                // Check if the country is actually added to the database
                var paysInDatabase = dbContext.Pays.FirstOrDefault(p => p.NomPays == newPays.NomPays);
                Assert.IsNotNull(paysInDatabase);
                Assert.AreEqual(newPays.IdPays, paysInDatabase.IdPays);
                Assert.AreEqual(newPays.NomPays, paysInDatabase.NomPays);

            }
        }

        [TestMethod]
        public async Task Put_Nations_Returns_Bad_Request_If_Nation_Doesnt_Exists()
        {
            // Arrange
            var pays = new Pays { IdPays = "GER", NomPays = "Germany" };
            var newPays = new Pays { IdPays = "UKR", NomPays = "Ukraine" };

            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                await controller.PostNations(pays);
                // Act
                var actionResult = await controller.PutNations(newPays);

                // Assert
                var badRequestResult = actionResult as BadRequestResult;
                Assert.IsNotNull(badRequestResult);
                Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
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
        public async Task Post_Categorie_Returns_Created_Response()
        {
            // Arrange
            var newType = new TypeProduit { Id = 1, Description = "ma desc" , Nom= "mon type"};
            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.PostCategories(newType);
                var createdAtActionResult = actionResult as OkResult;
                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status200OK, createdAtActionResult.StatusCode);

                // Check if the country is actually added to the database
                var paysInDatabase = dbContext.TypeProduits.FirstOrDefault(p => p.Nom == newType.Nom);
                Assert.IsNotNull(paysInDatabase);
                Assert.AreEqual(newType.Id, paysInDatabase.Id);
            }
        }

        [TestMethod]
        public async Task Post_Categorie_Returns_Bad_Request_If_Nation_Already_Exists()
        {
            // Arrange
            var type = new TypeProduit { Id = 1, Description = "ma desc", Nom = "mon type" };
            var newType = new TypeProduit { Id = 1, Description = "ma desc", Nom = "mon type" };

           

            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                // Attempt to add the same nation again
                await controller.PostCategories(type);
                var actionResult = await controller.PostCategories(newType);


                // Assert
                var badRequestResult = actionResult as BadRequestObjectResult;
                Assert.IsNotNull(badRequestResult);
                Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.AreEqual("category already in base", badRequestResult.Value);
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
        public async Task Post_Couleur_Returns_Created_Response()
        {
            // Arrange
            var newCouleur = new Couleur { Id = 1, Nom="rouge", Hexa = "FFFFFF" };
            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.PostCouleur(newCouleur);
                var createdAtActionResult = actionResult as OkResult;
                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status200OK, createdAtActionResult.StatusCode);

                // Check if the country is actually added to the database
                var paysInDatabase = dbContext.Couleurs.FirstOrDefault(p => p.Nom == newCouleur.Nom);
                Assert.IsNotNull(paysInDatabase);
                Assert.AreEqual(newCouleur.Id, paysInDatabase.Id);
            }
        }

        [TestMethod]
        public async Task Post_Couleur_Returns_Bad_Request_If_Nation_Already_Exists()
        {
            // Arrange
            var couleur = new Couleur { Id = 1, Nom = "rouge", Hexa = "FFFFFF" };
            var newCouleur = new Couleur { Id = 1, Nom = "rouge", Hexa = "FFFFFF" };




            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                // Attempt to add the same nation again
                await controller.PostCouleur(couleur);
                var actionResult = await controller.PostCouleur(newCouleur);


                // Assert
                var badRequestResult = actionResult as BadRequestObjectResult;
                Assert.IsNotNull(badRequestResult);
                Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.AreEqual("color already in base", badRequestResult.Value);
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

        [TestMethod]
        public async Task Post_Taille_Returns_Created_Response()
        {
            // Arrange
            var newTaille = new Taille { IdTaille = "XL", NomTaille = "Très large" };
            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);

                // Act
                var actionResult = await controller.PostTailles(newTaille);
                var createdAtActionResult = actionResult as OkResult;
                // Assert
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual(StatusCodes.Status200OK, createdAtActionResult.StatusCode);

                // Check if the country is actually added to the database
                var paysInDatabase = dbContext.Tailles.FirstOrDefault(p => p.NomTaille == newTaille.NomTaille);
                Assert.IsNotNull(paysInDatabase);
                Assert.AreEqual(newTaille.IdTaille, paysInDatabase.IdTaille);
            }
        }

        [TestMethod]
        public async Task Post_Taille_Returns_Bad_Request_If_Nation_Already_Exists()
        {
            // Arrange
            var taille = new Taille { IdTaille = "XL", NomTaille = "Très large" };
            var newTaille = new Taille { IdTaille = "XL", NomTaille = "Très large" };





            using (var dbContext = CreateDbContext())
            {
                var controller = new CategoriesController(dbContext);
                // Attempt to add the same nation again
                await controller.PostTailles(taille);
                var actionResult = await controller.PostTailles(newTaille);


                // Assert
                var badRequestResult = actionResult as BadRequestObjectResult;
                Assert.IsNotNull(badRequestResult);
                Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.AreEqual("size already in base", badRequestResult.Value);
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