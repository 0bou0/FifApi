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

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTests
    {
      


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


        private FifaDBContext CreateDbContext(List<Utilisateur> users)
        {
            var options = new DbContextOptionsBuilder<FifaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestLoginDatabase")
                .Options;

            var dbContext = new FifaDBContext(options);

            dbContext.Utilisateurs.AddRange(users);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
