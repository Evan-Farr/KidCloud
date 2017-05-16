using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KidCloudProject.Controllers;
using System.Web.Mvc;

namespace KidCloudUnitTest
{
    [TestClass]
    public class HomeUnitTests
    {
        [TestMethod]
        public void Controller_Home_Index_Get()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Home_About_Get()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Home_Contact_Get()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
