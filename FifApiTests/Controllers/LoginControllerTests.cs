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
using Newtonsoft.Json;
using NuGet.Protocol;

namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTests
    {
        private IConfiguration CreateConfigWithSecretKey()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Jwt:SecretKey", "40714B95E63F3FDFEB5AAB13BA512A5ACABAC9B9D01F7E0B617A15312347EB46" }, // Remplacez "votre_clé_secrète" par votre clé secrète réelle
            { "Jwt:Issuer", "https://localhost:5260/" },
            { "Jwt:Audience", "https://localhost:5260/" }
                })
                .Build();

            return config;
        }


        [TestMethod]
        public async Task ViewUtilisateur_WithValidToken_ShouldReturnUserDetails()
        {

            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4f6e95cdb04377ecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"

            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(utilisateur);
                Assert.IsNotNull(createUser.ToJson());


                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,

                };
                // Act
                var tokenActionResult = loginController.Login(user);
                int startIndex = tokenActionResult.ToJson().IndexOf("\"token\":\"") + "\"token\":\"".Length;
                int endIndex = tokenActionResult.ToJson().IndexOf("\"", startIndex);

                // Extraire la partie "token" de la chaîne JSON en utilisant Substring
                string tokenValue = tokenActionResult.ToJson().Substring(startIndex, endIndex - startIndex);
                Assert.IsNotNull(tokenValue);

                // Assert

                var result = tokenActionResult as OkObjectResult;

                Assert.IsNotNull(result);
                Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

                // Optional: Assert other properties of the response if needed
                var token = result.Value.GetType().GetProperty("token").GetValue(result.Value).ToString();
                Assert.IsNotNull(token);

            }
        }






        [TestMethod]
        public async Task Login_Invalid_User_Returns_Unauthorized_Without_Password()
        {
            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4f6e95cdb04377ecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"
            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(utilisateur);
                Assert.IsNotNull(createUser.ToJson());

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                };

                // Act
                try
                {
                    loginController.Login(user);
                    Assert.Fail("L'assertion a échoué : L'opération de connexion ne lève pas d'exception.");
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.IsTrue(ex is InvalidOperationException, "L'exception levée n'est pas du type attendu.");
                    Assert.AreEqual("Sequence contains no matching element", ex.Message, "Le message d'exception n'est pas celui attendu.");
                }
            }
        }



        [TestMethod]
        public async Task Login_Invalid_User_Returns_Unauthorized_Without_Name_and_mail()
        {
            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4f6e95cdb04377ecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"
            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(utilisateur);
                Assert.IsNotNull(createUser.ToJson());

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var user = new User
                {
                    Password = newUser.MotDePasse,
                };

                // Act
                try
                {
                    loginController.Login(user);
                    Assert.Fail("L'assertion a échoué : L'opération de connexion ne lève pas d'exception.");
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message, "Le message d'exception n'est pas celui attendu.");
                }

            }
        }


        [TestMethod]
        public async Task Login_Invalid_User_Returns_Unauthorized_With_False_Name()
        {
            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4f6e95cdb04377ecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"
            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(utilisateur);
                Assert.IsNotNull(createUser.ToJson());

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var user = new User
                {
                    UserName = "mauavis nom",
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                };

                // Act
                try
                {
                    loginController.Login(user);
                    Assert.Fail("L'assertion a échoué : L'opération de connexion ne lève pas d'exception.");
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.IsTrue(ex is InvalidOperationException, "L'exception levée n'est pas du type attendu.");
                    Assert.AreEqual("Sequence contains no matching element", ex.Message, "Le message d'exception n'est pas celui attendu.");
                }
            }
        }

      

        [TestMethod]
        public async Task Login_Invalid_User_Returns_Unauthorized_With_False_Password()
        {
            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4f6e95cdb04377ecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"
            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(utilisateur);
                Assert.IsNotNull(createUser.ToJson());

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = "mauvais password",
                };

                // Act
                try
                {
                    loginController.Login(user);
                    Assert.Fail("L'assertion a échoué : L'opération de connexion ne lève pas d'exception.");
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.IsTrue(ex is InvalidOperationException, "L'exception levée n'est pas du type attendu.");
                    Assert.AreEqual("Sequence contains no matching element", ex.Message, "Le message d'exception n'est pas celui attendu.");
                }
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
