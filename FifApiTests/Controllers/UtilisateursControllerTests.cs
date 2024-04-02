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











        private FifaDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<FifaDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new FifaDBContext(options);

           

            dbContext.SaveChanges();

            return dbContext;
        }




    }
}
