using ECore.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ECore.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexNotNullTest()
        {
            // Arrange
            var controller = new HomeController(null);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AboutViewDataTest()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.Equal("О проекте", result?.ViewData["Message"]);
        }
    }
}
