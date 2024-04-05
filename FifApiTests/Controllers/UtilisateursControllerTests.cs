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
                dbContext.Utilisateurs.AddRange(new[]
                {
                    new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", MailUtilisateur = "user1@example.com", MotDePasse = "password1", Role = "user" },
                    new Utilisateur { IdUtilisateur = 2, PseudoUtilisateur = "user2", MailUtilisateur = "user2@example.com", MotDePasse = "password2", Role = "user" }
                });
                dbContext.SaveChanges();

                var controller = new UtilisateursController(dbContext, null);

                var actionResult = await controller.GetUtilisateurs();
                var result = actionResult.Value.ToList();

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
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

                // Vérifiez si le résultat est une chaîne (string)
                Assert.IsInstanceOfType(result, typeof(string), "Le résultat devrait être une chaîne de caractères.");

                // Vérifiez si la chaîne représente un objet JSON avec la propriété "email"
                var jsonString = (string)result;
                dynamic jsonObject = JObject.Parse(jsonString);
                Assert.IsTrue(jsonObject.email != null, "Le résultat devrait contenir la propriété 'email'.");

                // Vérifiez si la valeur de la propriété "email" est correcte
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
        public async Task ViewUtilisateur_Returns_Unauthorized_When_Invalid_Token_Provided()
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
                    token = "coucou"
                };
                // Act

              




                var resultViewUti = utilisateurController.ViewUtilisateur(user);
                Assert.IsNotNull(resultViewUti);


                Assert.IsInstanceOfType(resultViewUti, typeof(Task<ActionResult<object>>));
                // Récupère la valeur de actionResult.Value pour faciliter les assertions ultérieures
                var createdResult = resultViewUti.Result;

                var returnUser = createdResult.Value;
                User reUser = JsonConvert.DeserializeObject<User>(createdResult.Value.ToJson());


                // Vérifier si reUser est de type UnauthorizedResult
                Assert.IsInstanceOfType(reUser.Password, typeof(UnauthorizedResult));

            }
        }

        [TestMethod]
        public async Task ViewUtilisateur_Returns_NotFound_When_User_Not_Found()
        {
            // Arrange
            var users = new List<Utilisateur>();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("test_secret_key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "testuser")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var generatedToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(generatedToken);

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { token = token };

                // Act
                var actionResult = await controller.ViewUtilisateur(user);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task ChangeUserName_Valid_User_Returns_Success() //-----------------------------------------------------------------------------------------------------
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { UserName = "newusername", Password = "password" };

                // Act
                var result = await controller.ChangeUserName(user);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(ActionResult<object>));

                var actionResult = result.Result;
                Assert.IsNotNull(actionResult);

                dynamic responseObject = actionResult;
                Assert.IsTrue(responseObject.changed);

                // Optionally, you can assert other properties of the response if needed
            }
        }

        [TestMethod]
        public async Task ChangeUserName_Invalid_User_Returns_NotFound()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { UserName = "nonexistentuser", Password = "password" };

                // Act
                var result = await controller.ChangeUserName(user);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }
        }


        [TestMethod]
        public async Task ChangePassword_Invalid_User_Returns_NotFound()
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { UserName = "invaliduser", Password = "password", NewPassword = "newpassword" };

                // Act
                var result = await controller.ChangePassword(user);

                // Assert
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task ChangePassword_Incorrect_Password_Returns_BadRequest()
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { UserName = "testuser", Password = "incorrectpassword", NewPassword = "newpassword" };

                // Act
                var result = await controller.ChangePassword(user);

                // Assert
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task ChangePassword_Valid_User_Returns_Success() //--------------------------------------------------------------------------------------------------------
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", Role = "user", MailUtilisateur = "test@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);
                var user = new User { UserName = "testuser", Password = "password", NewPassword = "newpassword" };

                // Act
                var result = await controller.ChangePassword(user);

                // Assert
                Assert.IsNotNull(result);

            }
        }
        [TestMethod]
        public async Task ChangeLastName_Updates_LastName_Successfully() // ------------------------------------------------------------------------------------------------------
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", NomUtilisateur = "Doe", MotDePasse = "password1", Role = "user", MailUtilisateur = "user1@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var user = new User { LastName = "Johnson" };

                var controller = new UtilisateursController(dbContext, null);

                // Act
                var actionResult = await controller.ChangeLastName(user);

                // Assert
                Assert.IsNotNull(actionResult);
                Assert.IsTrue(actionResult.Value is { });

                // Check if the last name has been updated successfully
                var updatedUser = await dbContext.Utilisateurs.FirstOrDefaultAsync(u => u.PseudoUtilisateur == "user1");
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual("Johnson", updatedUser.PrenomUtilisateur);
            }
        }


        [TestMethod]
        public async Task ChangeLastName_Returns_NotFound_When_No_Users()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var user = new User { LastName = "Johnson" };

                var controller = new UtilisateursController(dbContext, null);

                // Act
                var actionResult = await controller.ChangeLastName(user);

                // Assert
                Assert.IsNotNull(actionResult);

                var jsonBody = System.Text.Json.JsonSerializer.Serialize(actionResult.Value);
                var expectedJson = "{\"changed\":false}";
                Assert.AreEqual(expectedJson, jsonBody);
            }
        }

        [TestMethod]
        public async Task ChangeLastName_Returns_NotFound_When_User_Not_Found()
        {
            // Arrange
            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "user1", NomUtilisateur = "Doe", MotDePasse = "password1", Role = "user", MailUtilisateur = "user1@example.com" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var user = new User { LastName = "Johnson" }; // Changing the last name of a non-existing user

                var controller = new UtilisateursController(dbContext, null);

                // Act
                var actionResult = await controller.ChangeLastName(user);

                // Assert
                Assert.IsNotNull(actionResult);
                var jsonBody = System.Text.Json.JsonSerializer.Serialize(actionResult.Value);
                var expectedJson = "{\"changed\":false}";
                Assert.AreEqual(expectedJson, jsonBody);
            }
        }

        [TestMethod]
        public async Task ChangeFirstName_With_Valid_User_Returns_Success() //-------------------------------------------------------------------------------------------------
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                Password = "password",
                FirstName = "John"
            };

            var users = new List<Utilisateur>
    {
        new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MotDePasse = "password", PrenomUtilisateur = "Jane" }
    };

            using (var dbContext = CreateDbContext(users))
            {
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.ChangeFirstName(user);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

                var jsonBody = System.Text.Json.JsonSerializer.Serialize(result.Value);
                var expectedJson = "{\"changed\":true}";
                Assert.AreEqual(expectedJson, jsonBody);

                var okResult = result.Result as OkObjectResult;
                dynamic data = okResult.Value;
                Assert.IsTrue(data.changed);
            }
        }

        [TestMethod]
        public async Task ChangeFirstName_With_Invalid_User_Returns_NotFound()
        {
            // Arrange
            var user = new User
            {
                UserName = "invaliduser",
                Password = "invalidpassword",
                FirstName = "John"
            };

            using (var dbContext = CreateDbContext())
            {
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.ChangeFirstName(user);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task ChangeFirstName_With_Null_Context_Returns_NotFound()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                Password = "password",
                FirstName = "John"
            };

            using (var dbContext = CreateDbContext(null))
            {
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.ChangeFirstName(user);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
            }
        }


        [TestMethod]
        public async Task PutUtilisateur_ValidData_Returns_Ok()
        {
            // Arrange
            var userToUpdate = new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MailUtilisateur = "test@example.com", MotDePasse = "password", Role = "user" };
            var updatedUser = new User { UserName = "updateduser", Email = "updated@example.com", Password = "newpassword" };

            using (var dbContext = CreateDbContext(new List<Utilisateur> { userToUpdate }))
            {
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.PutUtilisateur(userToUpdate.IdUtilisateur, updatedUser);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ActionResult<object>));
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
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.PostUtilisateur(newUser);
                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ActionResult<object>));
                Assert.IsTrue(dbContext.Utilisateurs.Any(u => u.PseudoUtilisateur == newUser.UserName && u.MailUtilisateur == newUser.Email && u.MotDePasse == newUser.Password));
            }
        }

        [TestMethod]
        public async Task DeleteUtilisateur_ExistingUser_Returns_NoContent()
        {
            // Arrange
            var existingUser = new Utilisateur { IdUtilisateur = 1, PseudoUtilisateur = "testuser", MailUtilisateur = "test@example.com", MotDePasse = "password", Role = "user" };

            using (var dbContext = CreateDbContext(new List<Utilisateur> { existingUser }))
            {
                var controller = new UtilisateursController(dbContext, null);

                // Act
                var result = await controller.DeleteUtilisateur(new User { UserName = "testuser", Email = "test@example.com", Password = "password" });

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ActionResult<object>));
                Assert.AreEqual(StatusCodes.Status204NoContent, (result as StatusCodeResult)?.StatusCode);
                Assert.IsFalse(dbContext.Utilisateurs.Any(u => u.IdUtilisateur == existingUser.IdUtilisateur));
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
