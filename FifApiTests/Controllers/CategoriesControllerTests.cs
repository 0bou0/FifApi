using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FifApi.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FifApi.Models;
using Microsoft.AspNetCore.Http;

namespace FifApi.Controllers.Tests
{
    [TestClass()]
    public class CategoriesControllerTests
    {
        private CategoriesController _controller;
        private FifaDBContext _context;
        private Mock<FifaDBContext> _mockContext;
        private Mock<IDataRepository<Couleur>> _mockRepository;




        public CategoriesControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FifaDBContext>()
               .UseNpgsql("Server=projet-fifapi.postgres.database.azure.com;Database=postgres;Port=5432;User Id=s212;Password=bQ3i2%C$;Ssl Mode=Require;Trust Server Certificate=true;"); // Chaine de connexion à mettre dans les ( )
            _context = new FifaDBContext(builder.Options);
            _controller = new CategoriesController(_context);
            _mockRepository = new Mock<IDataRepository<Couleur>>();
            _mockContext = new Mock<FifaDBContext>();


        }




        [TestMethod]
        public async Task GetAllCouleur_ReturnTrue_WithMoq()
        {
            // Act
            var actionResult = await _controller.GetCouleurs();
            var statusCode = (actionResult.Result as StatusCodeResult)?.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult)); // Vérifie qu'aucune erreur 500 n'est retournée
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult)); // Vérifie qu'aucune erreur 404 n'est retournée
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult)); // Vérifie qu'aucune erreur 400 n'est retournée
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200); // Vérifie que le code de statut est 200
        }



    }
}