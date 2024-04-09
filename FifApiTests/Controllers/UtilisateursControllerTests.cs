using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using FifApi.Models;
using FifApi.Models.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using NuGet.ContentModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
namespace FifApi.Tests.Controllers
{
    [TestClass]
    public class UtilisateursControllerTests
    {


        [TestMethod]
        public async Task Get_Utilisateurs_Returns_All_Utilisateurs()
        {
            using (var dbContext = CreateDbContext())
            {
                List<Utilisateur> uts = new List<Utilisateur> {      new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
                    new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }};
                dbContext.Utilisateurs.AddRange(uts);
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.GetUtilisateurs();
                var result = actionResult.Value.ToList();

                Assert.IsNotNull(result);
                Assert.AreEqual(uts.Count, result.Count);
            }
        }

        [TestMethod]
        public async Task Get_Utilisateurs_Returns_All_Utilisateurs_Failed()
        {
            using (var dbContext = CreateDbContext())
            {
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.GetUtilisateurs();
                var result = actionResult.Value.ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task Chek_Email_Returns_Correct_Result()
        {
            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.AddRange(new[]
                {
            new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
            new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }
        });
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.ChekEmail("user1@example.com");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result, "Le résultat ne devrait pas être null.");

                Assert.IsInstanceOfType(result, typeof(string), "Le résultat devrait être une chaîne de caractères.");

                var jsonString = (string)result;
                dynamic jsonObject = JObject.Parse(jsonString);
                Assert.IsTrue(jsonObject.email != null, "Le résultat devrait contenir la propriété 'email'.");

                var emailAvailable = (bool)jsonObject.email;
                Assert.IsFalse(emailAvailable, "L'email devrait être disponible.");
            }
        }


        [TestMethod]
        public async Task Chek_Email_Returns_Wrong_Result()
        {
            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.AddRange(new[]
                {
            new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
            new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }
        });
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.ChekEmail("userfff@example.com");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result, "Le résultat ne devrait pas être null.");

                Assert.IsInstanceOfType(result, typeof(string), "Le résultat devrait être une chaîne de caractères.");

                var jsonString = (string)result;
                dynamic jsonObject = JObject.Parse(jsonString);
                Assert.IsTrue(jsonObject.email != null, "Le résultat devrait contenir la propriété 'email'.");

                var emailAvailable = (bool)jsonObject.email;
                Assert.IsTrue(emailAvailable, "L'email devrait être disponible.");


            }
        }




        [TestMethod]
        public async Task Chek_Username_Returns_Correct_Result()
        {
            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.AddRange(new[]
                {
            new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
            new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }
        });
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.ChekEmail("user1");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result, "Le résultat ne devrait pas être null.");

                // Vérifiez si le résultat est une chaîne (string)
                Assert.IsInstanceOfType(result, typeof(string), "Le résultat devrait être une chaîne de caractères.");

                // Vérifiez si la chaîne représente un objet JSON avec la propriété "email"
                var jsonString = (string)result;
                dynamic jsonObject = JObject.Parse(jsonString);
                Assert.IsTrue(jsonObject.email != null, "Le résultat devrait contenir la propriété 'user'.");

                // Vérifiez si la valeur de la propriété "email" est correcte
                var emailAvailable = (bool)jsonObject.email;
                Assert.IsTrue(emailAvailable, "L'Username devrait être disponible.");
            }
        }


        [TestMethod]
        public async Task Chek_Username_Returns_Wrong_Result()
        {
            using (var dbContext = CreateDbContext())
            {
                dbContext.Utilisateurs.AddRange(new[]
                {
            new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
            new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }
        });
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.ChekEmail("pureeeelecodedefou");
                var result = actionResult.Value;

                // Assert
                Assert.IsNotNull(result, "Le résultat ne devrait pas être null.");

                // Vérifiez si le résultat est une chaîne (string)
                Assert.IsInstanceOfType(result, typeof(string), "Le résultat devrait être une chaîne de caractères.");

                // Vérifiez si la chaîne représente un objet JSON avec la propriété "email"
                var jsonString = (string)result;
                dynamic jsonObject = JObject.Parse(jsonString);
                Assert.IsTrue(jsonObject.email != null, "Le résultat devrait contenir la propriété 'user'.");

                // Vérifiez si la valeur de la propriété "email" est correcte
                var emailAvailable = (bool)jsonObject.email;
                Assert.IsTrue(emailAvailable, "L'Username devrait être disponible.");
            }
        }

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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = tokenValue,

                };


                var resultViewUti = utilisateurController.ViewUtilisateur(user);
                Assert.IsNotNull(resultViewUti);


                // Vérifie si actionResult est de type ActionResult<object>
                Assert.IsInstanceOfType(resultViewUti, typeof(Task<ActionResult<object>>));



                // Récupère la valeur de actionResult.Value pour faciliter les assertions ultérieures
                var createdResult = resultViewUti.Result;

                var returnUser = createdResult.Value;
                User reUser = JsonConvert.DeserializeObject<User>(createdResult.Value.ToJson());

                Assert.AreEqual(user.UserName, reUser.UserName);
                Assert.AreEqual(user.Email, reUser.Email);

            }
        }




       

        [TestMethod]
        public async Task ViewUtilisateur_Returns_NotFound_When_User_Not_Found()
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

              






                var resultViewUti = utilisateurController.ViewUtilisateur(utilisateur);
                Assert.IsNotNull(resultViewUti);


                Assert.IsInstanceOfType(resultViewUti, typeof(Task<ActionResult<object>>));
                // Récupère la valeur de actionResult.Value pour faciliter les assertions ultérieures
                var createdResult = resultViewUti.Result;

                var returnUser = createdResult.Value;
                Assert.IsNull(returnUser);

            }
        }

        [TestMethod]
        public async Task ChangeUserName_Valid_User_Returns_Success() //-----------------------------------------------------------------------------------------------------
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


                user = new User
                {
                    UserName = "salut",
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = tokenValue,

                };


                var resultViewUti = utilisateurController.ChangeUserName(user);
                Assert.IsNotNull(resultViewUti);

                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsTrue((bool)anonymousResult);


            }
        }

        [TestMethod]
        public async Task ChangeUserName_Invalid_User_Returns_NotFound()
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

               

                var resultViewUti = utilisateurController.ChangeUserName(utilisateur);
                Assert.IsNotNull(resultViewUti);

                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsFalse((bool)anonymousResult);


            }
        }


        [TestMethod]
        public async Task ChangePassword_Invalid_User_Returns_NotFound()
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
                    Password = "fffff",

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
        public async Task ChangePassword_Incorrect_Password_Returns_BadRequest()
        {

            // Arrange
            var utilisateur = new User
            {
                UserName = "user",
                Password = "e3e21eb2ea077ec4fecab4a80bb787a0a6a3fd9e285e6b5c279d",
                Email = "coucou@gmail.com"

            };

            using (var dbContext = CreateDbContext())
            {
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

               

                var resultViewUti = utilisateurController.ChangePassword(utilisateur);
                Assert.IsNotNull(resultViewUti);

                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsFalse((bool)anonymousResult);


            }
        }

        [TestMethod]
        public async Task ChangePassword_Valid_User_Returns_Success() //--------------------------------------------------------------------------------------------------------
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    NewPassword = "fefezfzefzefegsthsr",
                    token = tokenValue,

                };


                var resultViewUti = utilisateurController.ChangePassword(user);
                Assert.IsNotNull(resultViewUti);


                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsTrue((bool)anonymousResult);


            }
        }
        [TestMethod]
        public async Task ChangeLastName_Updates_LastName_Successfully() // ------------------------------------------------------------------------------------------------------
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = tokenValue,
                    LastName = "dupiont",

                };


                var resultViewUti = utilisateurController.ChangeLastName(user);
                Assert.IsNotNull(resultViewUti);
                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsTrue((bool)anonymousResult);


            }
        }


        [TestMethod]
        public async Task ChangeLastName_Returns_NotFound_When_User_Not_Found()
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = "foufou",
                    LastName = "dupiont",

                };


                var resultViewUti = utilisateurController.ChangeLastName(user);
                Assert.IsNotNull(resultViewUti);
                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsFalse((bool)anonymousResult);


            }
        }

      
        [TestMethod]
        public async Task ChangeFirstName_With_Valid_User_Returns_Success() //-------------------------------------------------------------------------------------------------
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = tokenValue,
                    FirstName = "dupiont",

                };


                var resultViewUti = utilisateurController.ChangeFirstName(user);
                Assert.IsNotNull(resultViewUti);
                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsTrue((bool)anonymousResult);


            }
        }

        [TestMethod]
        public async Task ChangeFirstName_With_Invalid_User_Returns_NotFound()
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = "ffff",
                    FirstName = "dupiont",

                };


                var resultViewUti = utilisateurController.ChangeFirstName(user);
                Assert.IsNotNull(resultViewUti);
                var result = resultViewUti.Result.Value;
                var anonymousResult = result.GetType().GetProperty("changed").GetValue(result, null);
                Assert.IsFalse((bool)anonymousResult);


            }
        }



        [TestMethod]
        public async Task PutUtilisateur_ValidData_Returns_Ok()
        {
            // Arrange
            var usertoupdate = new User { UserName = "mon", Email = "up@example.com", Password = "pass" };
            var updatedUser = new User { UserName = "updateduser", Email = "updated@example.com", Password = "pass" , NewPassword = "new pass"};

            using (var dbContext = CreateDbContext())
            {
                // Act
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(usertoupdate);

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var updateUser = utilisateurController.PutUtilisateur(newUser.IdUtilisateur, updatedUser);

                // Assert
                Assert.IsNotNull(updateUser);
                Assert.IsInstanceOfType(updateUser.Result.Value, typeof(object));
                Assert.IsTrue(dbContext.Utilisateurs.Any(u => u.PseudoUtilisateur == updatedUser.UserName && u.MailUtilisateur == updatedUser.Email && u.MotDePasse == updatedUser.NewPassword));
            }
        }

        [TestMethod]
        public async Task PutUtilisateur_ValidData_Returns_Wrong_Password()
        {
            // Arrange
            var usertoupdate = new User { UserName = "mon", Email = "up@example.com", Password = "pass" };
            var updatedUser = new User { UserName = "updateduser", Email = "updated@example.com", Password = "newpassword" };

            using (var dbContext = CreateDbContext())
            {
                // Act
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(usertoupdate);

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var updateUser = utilisateurController.PutUtilisateur(newUser.IdUtilisateur, updatedUser);

                // Assert
                Assert.IsNotNull(updateUser);
                Assert.IsInstanceOfType(updateUser.Result.Value, typeof(object));
                Assert.IsTrue(dbContext.Utilisateurs.Any(u => u.PseudoUtilisateur == updatedUser.UserName && u.MailUtilisateur == updatedUser.Email && u.MotDePasse == updatedUser.Password));
            }
        }

        [TestMethod]
        public async Task PutUtilisateur_ValidData_Returns_No_Token()
        {
            // Arrange
            var usertoupdate = new User { UserName = "mon", Email = "up@example.com", Password = "pass" };
            var updatedUser = new User { UserName = "updateduser", Email = "updated@example.com", Password = "newpassword" };

            using (var dbContext = CreateDbContext())
            {
                // Act
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(usertoupdate);

                var usersTask = dbContext.Utilisateurs.ToListAsync();
                var users = usersTask.Result;
                var newUser = users.FirstOrDefault();
                Assert.IsNotNull(newUser.ToJson());

                var updateUser = utilisateurController.PutUtilisateur(newUser.IdUtilisateur, updatedUser);

                // Assert
                Assert.IsNotNull(updateUser);
                Assert.IsInstanceOfType(updateUser.Result.Value, typeof(object));
                Assert.IsTrue(dbContext.Utilisateurs.Any(u => u.PseudoUtilisateur == updatedUser.UserName && u.MailUtilisateur == updatedUser.Email && u.MotDePasse == updatedUser.Password));
            }
        }

        [TestMethod]
        public async Task PostUtilisateur_ValidData_Returns_Created()
        {
            // Arrange
            var newUser = new User { UserName = "newuser", Email = "new@example.com", Password = "password" };

            using (var dbContext = CreateDbContext())
            {

                // Act
                var _config = CreateConfigWithSecretKey();
                var loginController = new LoginController(dbContext, _config);
                var utilisateurController = new UtilisateursController(dbContext, _config);

                var createUser = utilisateurController.PostUtilisateur(newUser);
                Assert.IsNotNull(createUser);
                // Assert
                Assert.IsInstanceOfType(createUser.Result.Value, typeof(object));
                Assert.IsTrue(dbContext.Utilisateurs.Any(u => u.PseudoUtilisateur == newUser.UserName && u.MailUtilisateur == newUser.Email && u.MotDePasse == newUser.Password));
            }
        }

        [TestMethod]
        public async Task DeleteUtilisateur_ExistingUser_Returns_NoContent()
        { // Arrange
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


                user = new User
                {
                    UserName = newUser.PseudoUtilisateur,
                    Email = newUser.MailUtilisateur,
                    Password = newUser.MotDePasse,
                    token = tokenValue,

                };


                var resultViewUti = utilisateurController.DeleteUtilisateur(user);
                Assert.IsNotNull(resultViewUti);

                Assert.IsInstanceOfType(resultViewUti.Result, typeof(NoContentResult));

            }
        }


        private FifaDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<FifaDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new FifaDBContext(options);



            dbContext.SaveChanges();

            return dbContext;
        }

        private FifaDBContext CreateDbContext(List<Utilisateur> utilisateurs = null)
        {
            var options = new DbContextOptionsBuilder<FifaDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new FifaDBContext(options);

            // Ajouter des utilisateurs si une liste est fournie
            if (utilisateurs != null && utilisateurs.Any())
            {
                dbContext.Utilisateurs.AddRange(utilisateurs);
                dbContext.SaveChanges();
            }

            return dbContext;
        }




    }
}
