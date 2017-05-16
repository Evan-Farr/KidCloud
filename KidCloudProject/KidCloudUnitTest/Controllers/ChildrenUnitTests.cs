using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KidCloudProject.Controllers;
using System.Web.Mvc;

namespace KidCloudUnitTest.Controllers
{
    [TestClass]
    public class ChildrenUnitTests
    {
        [TestMethod]
        public void Controller_Children_Index_Get()
        {
            // Arrange
            ChildrenController controller = new ChildrenController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Children_Create_Get()
        {
            // Arrange
            ChildrenController controller = new ChildrenController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Children_Create_Post()
        {
            // Arrange
            ChildrenController controller = new ChildrenController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
