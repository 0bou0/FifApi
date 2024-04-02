using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using FifApi.Models;
using FifApi.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTests
    {
        [TestMethod]
        public async Task Login_Valid_User_Returns_Ok()
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var secretKey = Encoding.UTF8.GetBytes("your_secret_key");
                var securityKey = new SymmetricSecurityKey(secretKey);

                var _config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
        { "Jwt:SecretKey", Convert.ToBase64String(secretKey) },
        { "Jwt:Issuer", "your_issuer" },
        { "Jwt:Audience", "your_audience" }
                    })
                    .Build();

                var controller = new LoginController(dbContext, _config);
                var user = new User { UserName = "testuser", Password = "password" };

                // Act
                var result = controller.Login(user) as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

                // Optional: Assert other properties of the response if needed
                var token = result.Value.GetType().GetProperty("token").GetValue(result.Value).ToString();
                Assert.IsNotNull(token);

            }
        }



        [TestMethod]
        public async Task Login_Invalid_User_Returns_Unauthorized()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupGet(x => x["Jwt:SecretKey"]).Returns("test_secret_key");

            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new LoginController(dbContext, mockConfig.Object);
                var user = new User { UserName = "invaliduser", Password = "password" };

                // Act
                var result = controller.Login(user) as UnauthorizedResult;

                // Assert
                Assert.IsNotNull(result);
            }
        }


        private FifaDBContext CreateDbContext(List<Utilisateur> users = null)
        {
            var options = new DbContextOptionsBuilder<FifaDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new FifaDBContext(options);

            // Ajouter des utilisateurs si une liste est fournie
            if (users != null && users.Any())
            {
                dbContext.Utilisateurs.AddRange(users);
                dbContext.SaveChanges();
            }

            return dbContext;
        }

    }
}
