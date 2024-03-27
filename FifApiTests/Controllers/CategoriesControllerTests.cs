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


        public CategoriesControllerTests()
        {
        }


        /*

        [TestMethod]
        public async Task GetAllCouleur_ReturnNoError()
        {
            // Act
            var actionResult = await _controller.GetCouleurs();
            var statusCode = (actionResult.Result as StatusCodeResult)?.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
        }

        [TestMethod]
        public async Task GetAllCouleur_ReturnWrightElement_WithMoq()
        {
            // Act
            var actionResult = await _controller.GetCouleurs();
            var statusCode = (actionResult.Result as StatusCodeResult)?.StatusCode;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(ObjectResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            Assert.IsNotInstanceOfType(actionResult.Result, typeof(BadRequestResult));
            Assert.AreEqual(StatusCodes.Status200OK, actionResult.Result is StatusCodeResult ? ((StatusCodeResult)actionResult.Result).StatusCode : 200);
        }*/



    }
}