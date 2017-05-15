using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KidCloudProject.Controllers;
using System.Web.Mvc;



namespace KidCloudUnitTest.Controllers
{
    [TestClass]
    public class ApplicationsUnitTests
    {
        [TestMethod]
        public void Controller_Applications_Index_Get()
        {
            // Arrange
            ApplicationsController controller = new ApplicationsController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Applications_Details_Get()
        {
            // Arrange
            ApplicationsController controller = new ApplicationsController();
            int id = 2;

            // Act
            ViewResult result = controller.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
